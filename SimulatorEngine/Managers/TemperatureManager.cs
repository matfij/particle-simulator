using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public static class TemperatureManager
{
    private static readonly float _transferRatio = 0.09f;
    private static readonly float _minTransferThreshold = 0.1f;
    private static readonly Vector2[] _topLeftOffsets =
    [
        new(0, 1), new(-1, 0), new (0, -1),
    ];

    public static void TransferHeat(Dictionary<Vector2, Particle> particles)
    {
        foreach (var (position, particle) in particles)
        {
            foreach (Vector2 offset in _topLeftOffsets)
            {
                if (!particles.TryGetValue(Vector2.Add(position, offset), out Particle? neighbor))
                {
                    continue;
                }
                var tempDiff = particle.Temperature - neighbor.Temperature;
                if (tempDiff > _minTransferThreshold)
                {
                    particle.Temperature -= _transferRatio * tempDiff;
                    neighbor.Temperature += _transferRatio * tempDiff;
                }
            }

            foreach (var transition in particle.Transitions)
            {
                if (transition.Direction == PhaseTransitionDirection.Up && particle.Temperature > transition.Temperature
                    || transition.Direction == PhaseTransitionDirection.Down && particle.Temperature < transition.Temperature)
                {
                    if (transition.ResultKind == ParticleKind.None)
                    {
                        particles.Remove(position);
                        break;
                    }
                    particles[position] = ParticlesPool.GetParticle(transition.ResultKind);
                    particles[position].Temperature = particle.Temperature;
                }
            }
        }
    }
}
