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
        switch (kind)
        {
            case ParticleKind.Sand:
                return new SandParticle(position);
            case ParticleKind.Water:
                return new WaterParticle(position);
            case ParticleKind.Iron:
                return new IronParticle(position);
            case ParticleKind.Oxygen:
                return new OxygenParticle(position);
            default:
                throw new InvalidEnumArgumentException("Unsupported particle kind");
        }
    }
}
