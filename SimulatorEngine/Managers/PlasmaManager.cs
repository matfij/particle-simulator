using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorEngine.Managers;

public class PlasmaManager(float dt, float gravity)
{
    private readonly float _dt = dt;
    private readonly float _gravity = gravity;
    private readonly float _dxyRatio = 400f;
    private readonly float _dyMin = -1;
    private readonly Random _randomFactory = new();

    public Vector2 MovePlasma(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var newPosition = position;

        var dyMax = (int)(_dt * _gravity * _dxyRatio / particle.Density);
        dyMax = _randomFactory.Next(1, 1 + dyMax);

        for (var dy = _dyMin; dy < dyMax; dy++)
        {
            var xNudge = _randomFactory.Next(-1, 2);
            var newPositionCandidate = new Vector2(newPosition.X + xNudge, newPosition.Y - dy);

            if (!particles.TryGetValue(newPositionCandidate, out var collidingParticle))
            {
                newPosition = newPositionCandidate;
            }
            else if (collidingParticle.Body != ParticleBody.Gas || collidingParticle.Body != ParticleBody.Plasma)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        return newPosition;
    }
}
