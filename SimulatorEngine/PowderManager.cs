using System.Numerics;

namespace SimulatorEngine;

public class PowderManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly int[] _sideDisplacementDirections = [-1, 1];

    public Vector2 MovePowder(Particle particle, HashSet<Vector2> occupiedPositions)
    {
        var initialPosition = particle.Position;
        var newPosition = initialPosition;

        var gravityDisplacement = (int)(_dt * particle.GetDensity() * _gravity);

        for (int dy = 1; dy <= gravityDisplacement; dy++)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + dy);
            if (!occupiedPositions.Contains(newPositionCandidate))
            {
                newPosition = newPositionCandidate;
                continue;
            }
            break;
        }

        if (newPosition != initialPosition)
        {
            return newPosition;
        }

        Random.Shared.Shuffle(_sideDisplacementDirections);

        foreach (var direction in _sideDisplacementDirections)
        {
            Vector2 newPositionCandidate = new(initialPosition.X + direction, initialPosition.Y + 1);
            if (!occupiedPositions.Contains (newPositionCandidate))
            {
                return newPositionCandidate;
            }
        }

        return initialPosition;
    }
}
