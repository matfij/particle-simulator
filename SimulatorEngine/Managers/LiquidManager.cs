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

    public Vector2 MoveLiquid(Vector2 position, IParticle particle, Dictionary<Vector2, IParticle> particles)
    {
        var initialPosition = position;
        var newPosition = initialPosition;
        var particleData = ParticlesDataManager.GetParticleData(particle.Kind);

        var dyMax = (int)(_dt * _gravity * particleData.Density);
        for (var dy = 1; dy <= dyMax; dy++)
        {
            var xNudge = _randomFactory.Next(-1, 2);

            var nudgedDown = new Vector2(initialPosition.X + xNudge, initialPosition.Y + dy);

            if (!particles.TryGetValue(nudgedDown, out var fallingInto))
            {
                newPosition = nudgedDown;
            }
            else if (ParticleUtils.TryPushLighterParticle(particle, fallingInto, particles, nudgedDown))
            {
                return nudgedDown;
            }
            else if (ParticlesDataManager.GetParticleData(fallingInto.Kind).Body != ParticleBody.Liquid)
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
            var dxMax = (int)(_dt * _sideDisplacementFactor / particleData.Density);

            for (var dx = 1; dx <= dxMax; dx++)
            {
                var dxPosition = new Vector2(initialPosition.X + dx * direction, initialPosition.Y);

                if (!particles.TryGetValue(dxPosition, out var dxNeighbor))
                {
                    return dxPosition;
                }
                if (ParticlesDataManager.GetParticleData(dxNeighbor.Kind).Body != ParticleBody.Liquid)
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
