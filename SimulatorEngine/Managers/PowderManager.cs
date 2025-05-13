using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public class PowderManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly Random _randomFactory = new();

    public Vector2 MovePowder(Vector2 position, IParticle particle, Dictionary<Vector2, IParticle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;
        var particleData = ParticlesDataManager.GetParticleData(particle.Kind);

        var gravityDisplacement = (int)(_dt * particleData.Density * _gravity);

        for (int dy = 1; dy <= gravityDisplacement; dy++)
        {
            Vector2 newPositionCandidate = new(initialPosition.X, initialPosition.Y + dy);
            if (!particles.TryGetValue(newPositionCandidate, out IParticle? collidingParticle))
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
            if (!particles.TryGetValue(newPositionCandidate, out IParticle? collidingParticle))
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
}
