namespace SimulatorEngine;

public class ParticlesManager
{
    private HashSet<Particle> Particles = [];
    private readonly ParticlePool ParticlesPool = new();
    private int ParticlesCount = 0;

    public IEnumerable<Particle> GetParticles => Particles;

    public int GetParticlesCount => ParticlesCount;

    public void AddParticles((int x, int y) center, int radius, ParticleKind kind)
    {
        int radiusSquare = radius * radius;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx * dx + dy * dy <= radiusSquare)
                {
                    var particle = ParticlesPool.GetParticle(center.x + dx, center.y + dy, kind);
                    if (Particles.Contains(particle))
                    {
                        continue;
                    }
                    Particles.Add(particle);
                    ParticlesCount++;
                }
            }
        }
    }
}

