using System.ComponentModel;
using System.Numerics;

namespace SimulatorEngine;

public class ParticlesPool
{
    private readonly Stack<Particle> ParticlesFactory = new();

    public Particle GetParticle(Vector2 position, ParticleKind kind)
    {
        if (ParticlesFactory.Count > 0)
        {
            var particle = ParticlesFactory.Pop();
            return particle;
        }
        else
        {
            return CreateNewParticle(position, kind);
        }
    }

    public void ReturnParticle(Particle particle)
    {
        ParticlesFactory.Push(particle);
    }

    private static Particle CreateNewParticle(Vector2 position, ParticleKind kind)
    {
        return kind switch
        {
            ParticleKind.Sand => new SandParticle(position),
            ParticleKind.Water => new WaterParticle(position),
            ParticleKind.Iron => new IronParticle(position),
            ParticleKind.Oxygen => new OxygenParticle(position),
            _ => throw new InvalidEnumArgumentException("Unsupported particle kind"),
        };
    }
}
