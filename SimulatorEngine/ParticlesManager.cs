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
                var particle = ParticlesPool.GetParticle(center.x + dx, center.y + dy, kind);
                if (!Particles.Contains(particle) && dx * dx + dy * dy <= radiusSquare)
                {
                    Particles.Add(particle);
                    ParticlesCount++;
                }
            }
        }
    }

    public void RemoveParticles((int x, int y) center, int radius)
    {
        int radiusSquare = radius * radius;
        for (int dx = -radius; dx <= radius; dx ++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                var particle = ParticlesPool.GetParticle(center.x + dx, center.y + dy, ParticleKind.Sand);
                if (Particles.Contains(particle) && dx * dx + dy * dy <= radiusSquare)
                {
                    Particles.Remove(particle);
                    ParticlesCount--;
                }
            }
        }
    }
}
