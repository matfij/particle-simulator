using System.ComponentModel;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public static class ParticlesPool
{
    private static readonly Stack<IParticle> ParticlesFactory = new();

    public static IParticle GetParticle(ParticleKind kind)
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

    public static void ReturnParticle(IParticle particle)
    {
        ParticlesFactory.Push(particle);
    }

    private static IParticle CreateNewParticle(ParticleKind kind)
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
            ParticleKind.Stone => new StoneParticle(),
            ParticleKind.Steam => new SteamParticle(),
            _ => throw new InvalidEnumArgumentException("Unsupported particle kind"),
        };
    }
}
