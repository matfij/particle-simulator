using System.Numerics;

namespace SimulatorEngine;

public static class ParticleUtils
{
    public static bool TryPushLighterParticle(
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
