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
            else if (TryPushLighterParticle(particle, collidingParticle, particles, newPositionCandidate))
            {
                return newPositionCandidate;
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
            else if (TryPushLighterParticle(particle, collidingParticle, particles, newPositionCandidate))
            {
                return newPositionCandidate;
            }
        }

        return initialPosition;
    }

    private static bool TryPushLighterParticle(
        Particle particle,
        Particle collidingParticle,
        Dictionary<Vector2, Particle> particles,
        Vector2 newPositionCandidate)
    {
        if (collidingParticle.GetDensity() >= particle.GetDensity())
        {
            return false;
        }
        var pushUpPosition = new Vector2(newPositionCandidate.X, newPositionCandidate.Y - 1);
        while (particles.TryGetValue(pushUpPosition, out Particle? nextColliding) && pushUpPosition.Y > 0)
        {
            if (nextColliding?.Body == ParticleBody.Solid)
            {
                return false;
            }
            pushUpPosition.Y -= 1;
        }
        particles.Add(pushUpPosition, collidingParticle);
        particles.Remove(newPositionCandidate);
        return true;
    }
}
