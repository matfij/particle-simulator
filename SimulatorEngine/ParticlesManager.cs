namespace SimulatorEngine;

public class ParticlesManager
{
    private HashSet<Particle> Particles = [];


    public IEnumerable<Particle> GetParticles => Particles;


    public ParticlesManager()
    {
        var gen = new Random();

        for (int i = 0; i < 100; i++)
        {
            Particles.Add(new SandParticle(gen.Next(0, 1200), gen.Next(0, 600)));
        }
    }
}

