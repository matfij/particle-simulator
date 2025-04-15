using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public static class TemperatureManager
{
    private static readonly float _transferRatio = 0.025f;
    private static readonly float _minTransferThreshold = 0.1f;

    public static void TransferHeat(Dictionary<Vector2, Particle> particles)
    {
        foreach (var (position, particle) in particles)
        {
            var neighbors = ParticleUtils.GetStrictNeighbors(position, particles);

            foreach (var neighbor in neighbors)
            {
                var tempDiff = particle.Temperature - neighbor.Temperature;
                if (tempDiff < _minTransferThreshold)
                {
                    continue;
                }
                particle.Temperature -= _transferRatio * tempDiff;
                neighbor.Temperature += _transferRatio * tempDiff;
            }
        }

        foreach(var (position, particle) in particles)
        {
            foreach (var transition in particle.Transitions)
            {
                if (transition.Direction == PhaseTransitionDirection.Up && particle.Temperature > transition.Temperature
                    || transition.Direction == PhaseTransitionDirection.Down && particle.Temperature < transition.Temperature)
                {
                    particles[position] = ParticlesPool.GetParticle(transition.ResultKind);
                    particles[position].Temperature = particle.Temperature;
                }
                
            }
        }
    }
}
