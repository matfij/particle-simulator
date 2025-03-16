using SimulatorEngine;
using System.Numerics;

public class LiquidManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly float _sideDisplacementRatio = 1f;
    private readonly int[] _sideDisplacementDirections = [-1, 1];

    public Vector2 MoveLiquid(Particle particle, HashSet<Vector2> occupiedPositions, HashSet<Vector2> liquidPositions)
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
            }
            else if (!liquidPositions.Contains(newPositionCandidate))
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
            int maxSideDisplacement = (int)(_dt * particle.GetDensity() * _sideDisplacementRatio * _gravity);

            for (int dx = 1; dx <= maxSideDisplacement; dx++)
            {
                Vector2 sidePosition = new(initialPosition.X + dx * direction, initialPosition.Y);
                if (occupiedPositions.Contains(sidePosition) && !liquidPositions.Contains(sidePosition))
                {
                    break;
                }

                if (!occupiedPositions.Contains(sidePosition))
                {
                    Vector2 diagonalPosition = new(sidePosition.X, sidePosition.Y + 1);
                    if (!occupiedPositions.Contains(diagonalPosition))
                    {
                        newPosition = diagonalPosition;
                        continue;
                    }
                    newPosition = sidePosition;
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