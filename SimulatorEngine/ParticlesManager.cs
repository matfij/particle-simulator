using System;
using System.Timers;

namespace SimulatorEngine;

public class ParticlesManager
{
    private HashSet<Particle> Particles = [];
    private readonly ParticlesPool ParticlesPool = new();
    private int ParticlesCount = 0;
    private readonly System.Timers.Timer TickTimer = new(20);
    private bool ParticlesLock = false;
    private Random RandomFactory = new();

    public ParticlesManager()
    {
        TickTimer.Elapsed += (sender, args) => Tick();
        TickTimer.Start();
    }

    public IEnumerable<Particle> GetParticles => Particles;

    public int GetParticlesCount => ParticlesCount;

    public void AddParticles((int x, int y) center, int radius, ParticleKind kind)
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
                var particle = ParticlesPool.GetParticle(center.x + dx, center.y + dy, kind);
                if (!Particles.Contains(particle) && dx * dx + dy * dy <= radiusSquare)
                {
                    Particles.Add(particle);
                    ParticlesCount++;
                }
            }
        }

        ParticlesLock = false;
    }

    public void RemoveParticles((int x, int y) center, int radius, ParticleKind kind)
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
            int deltaFromCenter = (particle.X - center.x) * (particle.X - center.x) + (particle.Y - center.y) * (particle.Y - center.y);
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
                    int dx = RandomFactory.Next(0, 2) == 0 ? -1 : 1;
                    int dy = 3;

                    if (!Particles.Contains(ParticlesPool.GetParticle(particle.X, particle.Y + dy, particle.GetKind())))
                    {
                        particle.Y += dy;
                    }
                    else if (!Particles.Contains(ParticlesPool.GetParticle(particle.X + dx, particle.Y, particle.GetKind())))
                    {
                        particle.X += dx;
                    }
                    else if (!Particles.Contains(ParticlesPool.GetParticle(particle.X - dx, particle.Y, particle.GetKind())))
                    {
                        particle.X -= dx;
                    }

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
}
