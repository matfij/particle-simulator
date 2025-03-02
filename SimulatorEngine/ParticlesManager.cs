using System.Numerics;

namespace SimulatorEngine;

public class ParticlesManager
{
    private HashSet<Particle> Particles = [];
    private readonly ParticlesPool ParticlesPool = new();
    private int ParticlesCount = 0;
    private readonly System.Timers.Timer TickTimer = new(20);
    private bool ParticlesLock = false;

    public ParticlesManager()
    {
        TickTimer.Elapsed += (sender, args) => Tick();
        TickTimer.Start();
    }

    public IEnumerable<Particle> GetParticles => Particles;

    public int GetParticlesCount => ParticlesCount;

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
            for (int dy = -radius; dy <= radius; dy++)
            {
                var particle = ParticlesPool.GetParticle(new(center.X + dx, center.Y + dy), kind);
                if (!Particles.Contains(particle) && dx * dx + dy * dy <= radiusSquare)
                {
                    Particles.Add(particle);
                    ParticlesCount++;
                }
            }
        }

        ParticlesLock = false;
    }

    public void RemoveParticles(Vector2 center, int radius, ParticleKind kind)
    {
        if (ParticlesLock)
        {
            return;
        }
        ParticlesLock = true;

        HashSet<Particle> remainingParticles = [];
        ParticlesCount = 0;
        int radiusSquare = radius * radius;

        foreach (var particle in Particles)
        {
            var deltaFromCenter = Vector2.DistanceSquared(particle.Position, center);
            if (deltaFromCenter > radiusSquare)
            {
                remainingParticles.Add(particle);
                ParticlesCount++;
            }
        }

        Particles = remainingParticles;

        ParticlesLock = false;
    }

    private void Tick()
    {
        if (ParticlesLock)
        {
            return;
        }
        ParticlesLock = true;

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
        }

        ParticlesLock = false;
    }

    private void MoveLiquid(Particle particle)
    {
        particle.LastPosition = particle.Position;
        particle.Position += particle.Velocity;

        var velocity = particle.Position - particle.LastPosition;
        particle.Velocity = velocity;

        if (Particles.Contains(ParticlesPool.GetParticle(new(particle.Position.X + particle.Velocity.X, particle.Position.Y), particle.GetKind())))
        {
            particle.Velocity.X *= -1;
        }
        if (Particles.Contains(ParticlesPool.GetParticle(new(particle.Position.X, particle.Position.Y + particle.Velocity.Y), particle.GetKind())))
        {
            particle.Velocity.Y *= -1;
        }
    }
}
