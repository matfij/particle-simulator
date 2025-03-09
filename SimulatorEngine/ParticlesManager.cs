using System.Numerics;

namespace SimulatorEngine;

public class ParticlesManager
{
    private static readonly float Gravity = 0.001f;
    private static readonly float ReflectionDamping = 0.1f;
    private static readonly float VelocityDamping = 0.3f;
    private static readonly float RestDensity = 20f;
    private static readonly float KNear = 0.001f;
    private static readonly float K = 0.0005f;
    private static readonly float InteractionRadius = 8;
    private readonly (int Width, int Height) CanvasSize = (1200, 600);
    private readonly ParticlesPool ParticlesPool = new();
    private readonly ParticlesGrid ParticlesGrid = new();
    private List<Particle> Particles = [];
    private readonly System.Timers.Timer TickTimer = new(20);
    private bool ParticlesLock = false;

    public ParticlesManager()
    {
        TickTimer.Elapsed += (sender, args) => Tick();
        TickTimer.Start();
    }

    public IEnumerable<Particle> GetParticles => Particles;

    public int GetParticlesCount => Particles.Count;

    public void AddParticles(Vector2 center, int radius, ParticleKind kind)
    {
        if (ParticlesLock)
        {
            return;
        }
        ParticlesLock = true;

        int radiusSquare = radius * radius;
        for (int dx = -radius; dx <= radius; dx++)
        {
            if (center.X + dx > CanvasSize.Width || center.X + dx < 0)
            {
                continue;
            }
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (center.Y + dy > CanvasSize.Height || center.Y + dy < 0)
                {
                    continue;
                }
                Vector2 position = new(center.X + dx, center.Y + dy);
                if (!Particles.Contains(ParticlesPool.GetParticle(position, kind)) && dx * dx + dy * dy <= radiusSquare)
                {
                    Particles.Add(ParticlesPool.GetParticle(position, kind));
                }
            }
        }

        ParticlesGrid.SetParticles(Particles);

        ParticlesLock = false;
    }

    public void RemoveParticles(Vector2 center, int radius)
    {
        if (ParticlesLock)
        {
            return;
        }
        ParticlesLock = true;

        List<Particle> particlesToRemove = [];
        int radiusSquare = radius * radius;

        foreach (var particle in Particles)
        {
            var deltaFromCenter = Vector2.DistanceSquared(particle.Position, center);
            if (deltaFromCenter < radiusSquare)
            {
                particlesToRemove.Add(particle);
            }
        }

        foreach (var particle in particlesToRemove)
        {
            Particles.Remove(particle);
        }

        ParticlesGrid.SetParticles(Particles);

        ParticlesLock = false;
    }

    private void Tick()
    {
        if (ParticlesLock)
        {
            return;
        }
        ParticlesLock = true;

        List<Particle> particlesToRemove = [];

        ParticlesGrid.ClearGrid();
        ParticlesGrid.MapParticlesToCell();

        var particlesCount = Particles.Count;
        for (var i = 0; i < particlesCount; i++)
        {
            var particle = Particles[i];
            switch (particle.Body)
            {
                case ParticleBody.Gas:
                    // TODO
                    break;
                case ParticleBody.Liquid:
                    MoveLiquid(particle, i, 4f);
                    break;
                case ParticleBody.Powder:
                    // TODO
                    break;
                case ParticleBody.Solid:
                    // TODO
                    break;
            }

            if (particle.Position.X < 0 || particle.Position.X > CanvasSize.Width || particle.Position.Y < 0 || particle.Position.Y > CanvasSize.Height)
            {
                particlesToRemove.Add(particle);
            }
        }

        foreach (var particle in particlesToRemove)
        {
            Particles.Remove(particle);
        }

        ParticlesLock = false;
    }

    private void MoveLiquid(Particle particle, int index, float dt)
    {
        // gravity
        particle.Velocity.Y += dt * particle.GetDensity() * Gravity;

        // position prediction
        particle.LastPosition = particle.Position;
        var positionDelta = Vector2.Multiply(particle.Velocity, dt * VelocityDamping);
        particle.Position = Vector2.Add(particle.Position, positionDelta);

        // neighbor search
        var density = 0f;
        var densityNear = 0f;
        var neighbors = ParticlesGrid.GetNeighborOfParticleIndex(index);
        for (var i = 0; i < neighbors.Count; i++)
        {
            var neighbor = neighbors[i];
            if (particle.Position == neighbor.Position)
            {
                continue;
            }
            var positionGradient = Vector2.Subtract(neighbor.Position, particle.Position);
            var distance = positionGradient.Length() / InteractionRadius;
            if (distance < 1)
            {
                density += (float)Math.Pow(1 - distance, 2);
                densityNear += (float)Math.Pow(1 - distance, 3);
            }
        }
        var pressure = K * (density - RestDensity);
        var pressureNear = KNear * densityNear;
        var particleDisplacement = Vector2.Zero;
        for (var i = 0; i < neighbors.Count; i++)
        {
            var neighbor = neighbors[i];
            if (particle.Position == neighbor.Position)
            {
                continue;
            }
            var positionGradient = Vector2.Subtract(neighbor.Position, particle.Position);
            var distance = positionGradient.Length() / InteractionRadius;
            if (distance < 1)
            {
                positionGradient = Vector2.Normalize(positionGradient);
                var displacement = dt * dt * (pressure * (1 - distance) + pressureNear * (float)Math.Pow(1 - distance, 2));
                var dxy = new Vector2(displacement * positionGradient.X, displacement * positionGradient.Y);
                particleDisplacement -= Vector2.Multiply(dxy, 0.5f);
                neighbor.Position += Vector2.Multiply(dxy, 0.5f);
            }
        }
        particle.Position += particleDisplacement;

        // update velocity
        var velocity = (particle.Position - particle.LastPosition) / dt;
        particle.Velocity = velocity;

        // boundary
        if (particle.Position.X > CanvasSize.Width - 1 || particle.Position.X < 1)
        {
            particle.Position.X = Math.Clamp(particle.Position.X, 1, CanvasSize.Width - 1);
            particle.Velocity.X *= -ReflectionDamping;
        }
        if (particle.Position.Y > CanvasSize.Height - 1 || particle.Position.Y < 1)
        {
            particle.Position.Y = Math.Clamp(particle.Position.Y, 1, CanvasSize.Height - 1);
            particle.Velocity.Y *= -ReflectionDamping;
        }
    }
}
