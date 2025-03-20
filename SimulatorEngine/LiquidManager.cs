using System.Numerics;

namespace SimulatorEngine;

public class LiquidManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly float _sideDisplacementRatio = 0.8f;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly Random _randomFactory = new();

    public Vector2 MoveLiquid(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;

        var gravityDisplacement = (int)(_dt * particle.GetDensity() * _gravity);

        for (int dy = 1; dy <= gravityDisplacement; dy++)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + dy);
            if (!particles.ContainsKey(newPositionCandidate))
            {
                newPosition = newPositionCandidate;
            }
            else if (particles[newPositionCandidate].Body != ParticleBody.Liquid)
            {
                break;
            }
        }

        if (newPosition != initialPosition)
        {
            return newPosition;
        }

        _randomFactory.Shuffle(_sideDisplacementDirections);

        foreach (var direction in _sideDisplacementDirections)
        {
            int maxSideDisplacement = (int)(_dt * particle.GetDensity() * _sideDisplacementRatio * _gravity);

            for (int dx = 1; dx <= maxSideDisplacement; dx++)
            {
                Vector2 sidePosition = new(initialPosition.X + dx * direction, initialPosition.Y);
                if (particles.ContainsKey(sidePosition) && particles[sidePosition].Body != ParticleBody.Liquid)
                {
                    break;
                }
                if (particles.ContainsKey(sidePosition))
                {
                    continue;
                }

                Vector2 diagonalPosition = new(sidePosition.X, sidePosition.Y + _randomFactory.Next(1, 1 + dx));
                if (!particles.ContainsKey(diagonalPosition))
                {
                    newPosition = diagonalPosition;
                    continue;
                }
                newPosition = sidePosition;
            }

            if (newPosition != initialPosition)
            {
                return newPosition;
            }
        }

        return initialPosition;
    }
}
