using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorEngine.Managers;

public static class InteractionManager
{
    public static Particle? DoInteractions(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        foreach (var interaction in particle.Interactions)
        {
            if (interaction.Result == InteractionResult.ExpireTransform
                && ParticleUtils.GetNeighborOfKind(position, particles, particle.Kind) is not { })
            {
                interaction.Ticks--;
                if (interaction.Ticks <= 0)
                {
                    return ParticlesPool.GetParticle(interaction.ResultKind ?? ParticleKind.None);
                }
                continue;
            }
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
