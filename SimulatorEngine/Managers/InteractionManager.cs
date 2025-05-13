using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorEngine.Managers;

public static class InteractionManager
{
    public static IParticle? DoInteractions(Vector2 position, IParticle particle, Dictionary<Vector2, IParticle> particles)
    {
        var particleData = ParticlesDataManager.GetParticleData(particle.Kind);
        foreach (var interaction in particleData.Interactions)
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
                    return ParticlesPool.GetParticle(interaction.ResultKind ?? ParticleKind.None);
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
}
