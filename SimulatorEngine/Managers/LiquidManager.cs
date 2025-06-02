using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public class LiquidManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly float _sideDisplacementFactor = 100_000;
    private readonly int[] _sideDisplacementDirections = [-1, 1];
    private readonly Random _randomFactory = new();

    public Vector2 MoveLiquid(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;

        var dyMax = (int)(_dt * _gravity * particle.Density);
        for (var dy = 1; dy <= dyMax; dy++)
        {
            var xNudge = _randomFactory.Next(-1, 2);

            var yNudge = new Vector2(initialPosition.X + xNudge, initialPosition.Y + dy);

            if (!particles.TryGetValue(yNudge, out var fallingInto))
            {
                newPosition = yNudge;
            }
            else if (ParticleUtils.TryPushLighterParticle(particle, fallingInto, particles, yNudge))
            {
                return yNudge;
            }
            else if (fallingInto.Body != ParticleBody.Liquid)
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
            var dxMax = (int)(_dt * _sideDisplacementFactor / particle.Density);

            for (var dx = 1; dx <= dxMax; dx++)
            {
                var dxPosition = new Vector2(initialPosition.X + dx * direction, initialPosition.Y);

                if (!particles.TryGetValue(dxPosition, out var dxNeighbor))
                {
                    return dxPosition;
                }
                if (dxNeighbor.Body != ParticleBody.Liquid)
                {
                    break;
                }
                if (ParticleUtils.TryPushLighterParticle(particle, dxNeighbor, particles, dxPosition))
                {
                    return dxPosition;
                }
            }
        }

        return initialPosition;
    }

}
