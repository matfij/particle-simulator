using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorEngine.Managers;

public static class InteractionManager
{
    public static IParticle? DoInteractions(Vector2 position, IParticle particle, Dictionary<Vector2, IParticle> particles)
    {
        var particleData = ParticlesDataManager.GetParticleData(particle.Kind);
        foreach (var (interaction, index) in particleData.Interactions.Select((value, index) => ( value, index )))
        {
            if (ParticleUtils.GetNeighborOfKind(position, particles, interaction.NeighborKind) is not { } neighbor)
            {
                continue;
            }
            particle.InteractionTicks[index]--;
            if (particle.InteractionTicks[index] > 0)
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
