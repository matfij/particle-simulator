using System.Numerics;

namespace SimulatorEngine;

public class ParticlesManager
{
    private static readonly float Gravity = 0.05f;
    private readonly (int Width, int Height) CanvasSize = (1200, 600);
    private readonly ParticlesPool ParticlesPool = new();
    private HashSet<Particle> Particles = [];
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
        List<(Particle, Vector2)> movedParticles = [];
        HashSet<Vector2> occupiedPositions = [];

        var particlesCount = Particles.Count;
        foreach (var particle in Particles)
        {
            Vector2 newPosition = particle.Position;

            switch (particle.Body)
            {
                case ParticleBody.Gas:
                    break;
                case ParticleBody.Liquid:
                    newPosition = MoveLiquid(particle, occupiedPositions, 0.2f);
                    break;
                case ParticleBody.Powder:
                    break;
                case ParticleBody.Solid:
                    break;
            }

            if (particle.Position.X < 0 || particle.Position.X > CanvasSize.Width || particle.Position.Y < 0 || particle.Position.Y > CanvasSize.Height)
            {
                particlesToRemove.Add(particle);
            }
            if (newPosition != particle.Position)
            {
                movedParticles.Add((particle, newPosition));
                occupiedPositions.Add(newPosition);
            }
        }

        foreach (var (particle, newPosition) in movedParticles)
        {
            Particles.Remove(particle);
            particle.Position = newPosition;
            Particles.Add(particle);
        }
        foreach (var particle in particlesToRemove)
        {
            Particles.Remove(particle);
        }

        ParticlesLock = false;
    }

    private Vector2 MoveLiquid(Particle particle, HashSet<Vector2> occupiedPositions, float dt)
    {
        var lastPosition = particle.Position;

        var gravityDisplacement = (int)(dt * particle.GetDensity() * Gravity);

        for (int i = gravityDisplacement; i > 0; i--)
        {
            Vector2 newPosition = new(lastPosition.X, lastPosition.Y + i);
            if (
                !Particles.Contains(ParticlesPool.GetParticle(newPosition, particle.GetKind()))
                && !occupiedPositions.Contains(newPosition)
                )
            {
                return newPosition;
            }
        }


        int[] displacements = [-5, 5];
        Random.Shared.Shuffle(displacements);

        foreach (var displacement in displacements)
        {
            for (int dx = displacement; Math.Abs(dx) > 0;)
            {
                for (int dy = Math.Abs(displacement); dy > 0; dy--)
                {
                    Vector2 newPosition = new(lastPosition.X + dx, lastPosition.Y + dy);
                    if (
                   !Particles.Contains(ParticlesPool.GetParticle(newPosition, particle.GetKind()))
                    && !occupiedPositions.Contains(newPosition)
                   )
                    {
                        return newPosition;
                    }
                }
                if (displacement > 0) { dx--; } else { dx++; }
            }
        }

        foreach (var displacement
            in displacements)
        {
            for (int dx = displacement; Math.Abs(dx) > 0;)
            {
                Vector2 newPosition = new(lastPosition.X + dx, lastPosition.Y);
                if (
                    !Particles.Contains(ParticlesPool.GetParticle(newPosition, particle.GetKind()))
                     && !occupiedPositions.Contains(newPosition)
                    )
                {
                    return newPosition;
                }
                if (displacement > 0) { dx--; } else { dx++; }
            }
        }

        return lastPosition;
    }
}
