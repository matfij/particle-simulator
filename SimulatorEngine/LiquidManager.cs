using System.Numerics;

namespace SimulatorEngine;

public class LiquidManager(float gravity)
{
    private readonly float _sideDisplacementRatio = 0.75f;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly float _gravity = gravity;

    public Vector2 MoveLiquid(Particle particle, HashSet<Vector2> occupiedPositions, float dt)
    {
        var initialPosition = particle.Position;
        var newPosition = particle.Position;

        var gravityDisplacement = (int)(dt * particle.GetDensity() * _gravity);

        for (int dy = 1; dy <= gravityDisplacement; dy++)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + dy);
            if (!occupiedPositions.Contains(newPositionCandidate))
            {
                newPosition = newPositionCandidate;
            }
            else
            {
                break;
            }
        }
        if (newPosition != initialPosition)
        {
            return newPosition;
        }

        Random.Shared.Shuffle(_sideDisplacementDirections);

        foreach (var direction in _sideDisplacementDirections)
        {
            var sideDisplacement = (int)(dt * particle.GetDensity() * _sideDisplacementRatio * _gravity);

            for (int dx = 1; dx <= Math.Abs(sideDisplacement); dx++)
            {
                for (int dy = 0; dy <= Math.Abs(sideDisplacement); dy++)
                {
                    Vector2 newPositionCandidate = new(initialPosition.X + dx * direction, initialPosition.Y + dy);
                    if (!occupiedPositions.Contains(newPositionCandidate))
                    {
                        newPosition = newPositionCandidate;
                    }
                    else
                    {
                        break;
                    }
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
