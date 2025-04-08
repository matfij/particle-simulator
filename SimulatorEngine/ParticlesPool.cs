using System.ComponentModel;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public static class ParticlesPool
{
    private static readonly Stack<Particle> ParticlesFactory = new();

    public static Particle GetParticle(ParticleKind kind)
    {
        if (ParticlesFactory.Count > 0)
        {
            var particle = ParticlesFactory.Pop();
            return particle;
        }
        else
        {
            return CreateNewParticle(kind);
        }
    }

    public static void ReturnParticle(Particle particle)
    {
        ParticlesFactory.Push(particle);
    }

    private static Particle CreateNewParticle(ParticleKind kind)
    {
        return kind switch
        {
            ParticleKind.Sand => new SandParticle(),
            ParticleKind.Water => new WaterParticle(),
            ParticleKind.Iron => new IronParticle(),
            ParticleKind.Oxygen => new OxygenParticle(),
            ParticleKind.Salt => new SaltParticle(),
            ParticleKind.SaltyWater => new SaltyWaterParticle(),
            ParticleKind.Acid => new AcidParticle(),
            ParticleKind.Lava => new LavaParticle(),
            _ => throw new InvalidEnumArgumentException("Unsupported particle kind"),
        };
    }
}
