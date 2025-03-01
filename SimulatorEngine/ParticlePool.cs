using System.ComponentModel;
using SimulatorEngine;

namespace SimulatorEngine;

public class ParticlePool
{
    private readonly Stack<Particle> _pool = new();

    public Particle GetParticle(int x, int y, ParticleKind kind)
    {
        if (_pool.Count > 0)
        {
            var particle = _pool.Pop();
            particle.X = x;
            particle.Y = y;
            return particle;
        }
        else
        {
            return CreateNewParticle(x, y, kind);
        }
    }

    public void ReturnParticle(Particle particle)
    {
        _pool.Push(particle);
    }

    private Particle CreateNewParticle(int x, int y, ParticleKind kind)
    {
        switch (kind)
        {
            case ParticleKind.Sand:
                return new SandParticle(x, y);
            case ParticleKind.Water:
                return new WaterParticle(x, y);
            case ParticleKind.Iron:
                return new IronParticle(x, y);
            case ParticleKind.Oxygen:
                return new OxygenParticle(x, y);
            default:
                throw new InvalidEnumArgumentException("Unsupported particle kind");
        }
    }
}
