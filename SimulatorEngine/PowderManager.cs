using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public class PowderManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly Random _randomFactory = new();

    public Vector2 MovePowder(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var stateChanged = HandleStateChange(position, particle, particles);
        if (stateChanged)
        {
            return position;
        }

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
            else if (ParticleUtils.TryPushLighterParticle(particle, collidingParticle, particles, newPositionCandidate))
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
            else if (ParticleUtils.TryPushLighterParticle(particle, collidingParticle, particles, newPositionCandidate))
            {
                return newPositionCandidate;
            }
        }

        return initialPosition;
    }

    private static bool HandleStateChange(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        if (particle is SaltParticle saltParticle)
        {
            if (ParticleUtils.GetNeighborOfKind(position, particles, ParticleKind.Water) is not { } neighbor)
            {
                saltParticle.TicksToDissolve++;
                return false;
            }
            var (neighborPosition, _) = neighbor;
            saltParticle.TicksToDissolve--;
            if (saltParticle.TicksToDissolve <= 0)
            {
                particles.Remove(neighborPosition);
                particles.Remove(position);
                particles.Add(neighborPosition, new SaltyWaterParticle());
                particles.Add(position, new SaltyWaterParticle());
                return true;
            }
        }
        return false;
    }
}
