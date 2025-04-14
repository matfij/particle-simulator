using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public class GasManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly float _dxyRatio = 1000f;
    private readonly (int X, int Y)[] _displacementDirections = [(-1, 1), (1, -1), (1, 1), (-1, -1)];
    private readonly Random _randomFactory = new();

    public Vector2 MoveGas(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;

        var maxDisplacementValue = (int)(_dt * _gravity * _dxyRatio / particle.GetDensity());
        maxDisplacementValue = _randomFactory.Next(1, 1 + maxDisplacementValue);

        _randomFactory.Shuffle(_displacementDirections);

        foreach (var (X, Y) in _displacementDirections)
        {
            for (var dx = 1; dx <= maxDisplacementValue; dx++)
            {
                for (var dy = 1; dy <= maxDisplacementValue; dy++)
                {
                    Vector2 newPositionCandidate = new(initialPosition.X + X * dx, initialPosition.Y + Y * dy);
                    if (!particles.ContainsKey(newPositionCandidate))
                    {
                        newPosition = newPositionCandidate;
                    }
                    else if (newPosition != initialPosition)
                    {
                        return newPosition;
                    }
                    else
                    {
                        break;
                    }
                }
                if (newPosition == initialPosition)
                {
                    break;
                }
            }
            if (newPosition != initialPosition)
            {
                return newPosition;
            }
        }

        return initialPosition;
    }
}
