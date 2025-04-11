using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public static class TemperatureManager
{
    public static readonly float _transferRatio = 0.015f;

    public static void TransferHeat(Dictionary<Vector2, Particle> particles)
    {
        foreach (var (position, particle) in particles)
        {
            var neighbors = ParticleUtils.GetStrictNeighbors(position, particles);

            foreach (var neighbor in neighbors)
            {
                var tempDiff = particle.Temperature - neighbor.Temperature;
                if (tempDiff == 0)
                {
                    continue;
                }
                particle.Temperature -= _transferRatio * tempDiff;
                neighbor.Temperature += _transferRatio * tempDiff;
            }
        }
    }
}
