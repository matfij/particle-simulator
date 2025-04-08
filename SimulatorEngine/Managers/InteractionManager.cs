using SimulatorEngine.Particles;
using System.ComponentModel;
using System.Numerics;

namespace SimulatorEngine.Managers;

public static class InteractionManager
{
    public static Particle? DoInteractions(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        foreach (var interaction in particle.Interactions)
        {
            if (ParticleUtils.GetNeighborOfKind(position, particles, interaction.NeighborKind) is not { } neighbor)
            {
                continue;
            }
            interaction.Ticks--;
            if (interaction.Ticks > 0)
            {
                continue;
            }
            var (neighborPosition, _) = neighbor;
            switch (interaction.Result)
            {
                case InteractionResult.Merge:
                    particles.Remove(neighborPosition);
                    return CreateParticleOfKind(interaction.ResultKind ?? ParticleKind.None);
                case InteractionResult.RemoveSelf:
                    return null;
                case InteractionResult.RemoveNeighbor:
                    particles.Remove(neighborPosition);
                    return particle;
                case InteractionResult.RemoveBoth:
                    particles.Remove(neighborPosition);
                    return null;
            }
        }
        return particle;
    }

    private static Particle CreateParticleOfKind(ParticleKind particleKind)
    {
        return particleKind switch
        {
            ParticleKind.Sand => new SandParticle(),
            ParticleKind.Water => new WaterParticle(),
            ParticleKind.Iron => new IronParticle(),
            ParticleKind.Oxygen => new OxygenParticle(),
            ParticleKind.Salt => new SaltParticle(),
            ParticleKind.SaltyWater => new SaltyWaterParticle(),
            ParticleKind.Acid => new AcidParticle(),
            _ => throw new InvalidEnumArgumentException("Unsupported particle kind"),
        };
    }
}
