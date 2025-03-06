using System.Numerics;

namespace SimulatorEngine;

public class ParticlesManager
{
    private static readonly float Gravity = 0.0001f;
    private static readonly float ReflectionDamping = 0.8f;
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

        foreach (var particle in Particles)
        {
            switch (particle.Body)
            {
                case ParticleBody.Gas:
                    // TODO
                    break;
                case ParticleBody.Liquid:
                    MoveLiquid(particle);
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

    private void MoveLiquid(Particle particle)
    {
        // gravity
        particle.Velocity.Y += particle.GetDensity() * Gravity;

        // position prediction
        particle.LastPosition = particle.Position;
        particle.Position += particle.Velocity;

        // neighbor search

        // TODO - double density relaxation

        // update velocity
        var velocity = particle.Position - particle.LastPosition;
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
