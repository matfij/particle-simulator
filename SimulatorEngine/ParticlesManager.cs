namespace SimulatorEngine;

public class ParticlesManager
{
    private HashSet<Particle> Particles = [];
    private readonly ParticlePool ParticlePool = new();

    public IEnumerable<Particle> GetParticles => Particles;

    public void AddParticles((int x, int y) center, int radius, ParticleKind kind)
    {
        int radiusSquare = radius * radius;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx * dx + dy * dy <= radiusSquare)
                {
                    var particle = ParticlePool.GetParticle(center.x + dx, center.y + dy, kind);
                    Particles.Add(particle);
                }
            }
        }
    }
}

