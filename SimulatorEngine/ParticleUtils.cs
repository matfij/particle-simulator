using System.Numerics;
using System.Text;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public static class ParticleUtils
{
    private static readonly Vector2[] _strictNeighborOffsets =
    [
        new(0, 1),
        new(-1, 0), new(1, 0),
        new(0, -1),
    ];
    private static readonly Vector2[] _neighborOffsets =
    [
        .._strictNeighborOffsets,
        new(-1, 1),  new(1, 1),
        new(-1, -1), new(1, -1),
    ];

    public static bool TryPushLighterParticle(
        Particle particle,
        Particle collidingParticle,
        Dictionary<Vector2, Particle> particles,
        Vector2 newPositionCandidate)
    {
        if (collidingParticle.Body == ParticleBody.Solid || collidingParticle.Density >= particle.Density)
        {
            return false;
        }

        var pushUpPosition = new Vector2(newPositionCandidate.X, newPositionCandidate.Y - 1);

        while (particles.TryGetValue(pushUpPosition, out Particle? nextColliding))
        {
            if (nextColliding?.Body == ParticleBody.Solid || pushUpPosition.Y < 0)
            {
                return false;
            }
            pushUpPosition.Y -= 1;
        }

        particles.Add(pushUpPosition, collidingParticle);
        particles.Remove(newPositionCandidate);

        return true;
    }

    public static IEnumerable<Particle> GetStrictNeighbors(Vector2 position, Dictionary<Vector2, Particle> particles)
    {
        foreach (var offset in _strictNeighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out Particle? neighbor))
            {
                yield return neighbor;
            }
        }
    }

    public static IEnumerable<Particle> GetNeighbors(Vector2 position, Dictionary<Vector2, Particle> particles)
    {
        foreach (var offset in _neighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out Particle? neighbor))
            {
                yield return neighbor;
            }
        }
    }

    public static (Vector2, Particle)? GetNeighborOfKind(
        Vector2 position,
        Dictionary<Vector2, Particle> particles,
        ParticleKind kind)
    {
        foreach (var offset in _strictNeighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out Particle? neighbor) && neighbor.Kind == kind)
            {
                return (neighborPosition, neighbor);
            }
        }
        return null;
    }
}
