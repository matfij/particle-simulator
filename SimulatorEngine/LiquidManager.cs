using System.Numerics;

namespace SimulatorEngine;

public class LiquidManager(float gravity)
{
    private readonly int[] _sideDisplacementFactors = [-5, 5];
    private readonly float _gravity = gravity;

    public Vector2 MoveLiquid(Particle particle, HashSet<Vector2> occupiedPositions, float dt)
    {
        var initialPosition = particle.Position;
        var newPosition = particle.Position;

        MoveDown(ref newPosition, initialPosition, occupiedPositions, dt, particle);
        if (newPosition != initialPosition)
        {
            return newPosition;
        }

        Random.Shared.Shuffle(_sideDisplacementFactors);

        foreach (var displacement in _sideDisplacementFactors)
        {
            MoveSideDown(ref newPosition, displacement, initialPosition, occupiedPositions);
            if (newPosition != initialPosition)
            {
                return newPosition;
            }
            MoveSide(ref newPosition, displacement, initialPosition, occupiedPositions);
            if (newPosition != initialPosition)
            {
                return newPosition;
            }
        }

        return newPosition;
    }

    private void MoveDown(ref Vector2 newPosition, Vector2 initialPosition, HashSet<Vector2> occupiedPositions, float dt, Particle particle)
    {
        var gravityDisplacement = (int)(dt * particle.GetDensity() * _gravity);

        for (int i = gravityDisplacement; i > 0; i--)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + i);
            CheckNewPosition(ref newPosition, newPositionCandidate, occupiedPositions);
        }
    }

    private static void MoveSideDown(ref Vector2 newPosition, int displacement, Vector2 lastPosition, HashSet<Vector2> occupiedPositions)
    {
        for (int dx = Math.Abs(displacement); dx > 0; dx--)
        {
            for (int dy = Math.Abs(displacement); dy > 0; dy--)
            {
                var newX = lastPosition.X + dx * Math.Sign(displacement);
                Vector2 newPositionCandidate = new(newX, lastPosition.Y);
                CheckNewPosition(ref newPosition, newPositionCandidate, occupiedPositions);
            }
        }
    }

    private static void MoveSide(ref Vector2 newPosition, int displacement, Vector2 lastPosition, HashSet<Vector2> occupiedPositions)
    {
        for (int dx = Math.Abs(displacement); dx > 0; dx--)
        {
            var newX = lastPosition.X + dx * Math.Sign(displacement);
            Vector2 newPositionCandidate = new(newX, lastPosition.Y);
            CheckNewPosition(ref newPosition, newPositionCandidate, occupiedPositions);
        }
    }

    private static void CheckNewPosition(ref Vector2 newPosition, Vector2 newPositionCandidate, HashSet<Vector2> occupiedPositions)
    {
        if (!occupiedPositions.Contains(newPositionCandidate))
        {
            newPosition = newPositionCandidate;
        }
    }
}
