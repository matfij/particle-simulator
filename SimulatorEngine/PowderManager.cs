using System.Numerics;

namespace SimulatorEngine;

public class PowderManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly Random _randomFactory = new();

    public Vector2 MovePowder(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;

        var gravityDisplacement = (int)(_dt * particle.GetDensity() * _gravity);

        for (int dy = 1; dy <= gravityDisplacement; dy++)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + dy);
            if (!particles.TryGetValue(newPositionCandidate, out Particle? collidingParticle))
            {
                newPosition = newPositionCandidate;
                continue;
            }
            else if (collidingParticle.GetDensity() < particle.GetDensity())
            {
                particles.Remove(newPositionCandidate);
                newPosition = newPositionCandidate;
                var pushUpPosition = new Vector2(newPositionCandidate.X, newPositionCandidate.Y - 1);
                while (particles.ContainsKey(pushUpPosition) && pushUpPosition.Y > 0)
                {
                    pushUpPosition.Y -= 1;
                }
                if (pushUpPosition.Y >= 0)
                {
                    particles.Add(pushUpPosition, collidingParticle);
                }
                break;
            }
            break;
        }

        if (newPosition != initialPosition)
        {
            return newPosition;
        }

        _randomFactory.Shuffle(_sideDisplacementDirections);

        foreach (var direction in _sideDisplacementDirections)
        {
            Vector2 newPositionCandidate = new(initialPosition.X + direction, initialPosition.Y + 1);
            if (!particles.TryGetValue(newPositionCandidate, out Particle? collidingParticle))
            {
                return newPositionCandidate;
            }
            else if (collidingParticle.GetDensity() < particle.GetDensity())
            {
                particles.Remove(newPositionCandidate);
                var pushUpPosition = new Vector2(newPositionCandidate.X, newPositionCandidate.Y - 1);
                while (particles.ContainsKey(pushUpPosition) && pushUpPosition.Y > 0)
                {
                    pushUpPosition.Y -= 1;
                }
                if (pushUpPosition.Y >= 0)
                {
                    particles.Add(pushUpPosition, collidingParticle);
                }
                return newPositionCandidate;
            }
        }

        return initialPosition;
    }
}
