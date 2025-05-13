using System.Numerics;
using System.Text;
using SimulatorEngine.Managers;
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
        IParticle particle,
        IParticle collidingParticle,
        Dictionary<Vector2, IParticle> particles,
        Vector2 newPositionCandidate)
    {
        var particleData = ParticlesDataManager.GetParticleData(particle.Kind);
        var collidingParticleData = ParticlesDataManager.GetParticleData(collidingParticle.Kind);

        if (collidingParticleData.Density >= particleData.Density)
        {
            return false;
        }

        var pushUpPosition = new Vector2(newPositionCandidate.X, newPositionCandidate.Y - 1);

        while (particles.TryGetValue(pushUpPosition, out IParticle? nextColliding))
        {
            var nextCollidingData = ParticlesDataManager.GetParticleData(nextColliding.Kind);
            if (nextCollidingData.Body == ParticleBody.Solid || pushUpPosition.Y < 0)
            {
                return false;
            }
            pushUpPosition.Y -= 1;
        }

        particles.Add(pushUpPosition, collidingParticle);
        particles.Remove(newPositionCandidate);

        return true;
    }

    public static IEnumerable<IParticle> GetStrictNeighbors(Vector2 position, Dictionary<Vector2, IParticle> particles)
    {
        foreach (var offset in _strictNeighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out IParticle? neighbor))
            {
                yield return neighbor;
            }
        }
    }

    public static IEnumerable<IParticle> GetNeighbors(Vector2 position, Dictionary<Vector2, IParticle> particles)
    {
        foreach (var offset in _neighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out IParticle? neighbor))
            {
                yield return neighbor;
            }
        }
    }

    public static (Vector2, IParticle)? GetNeighborOfKind(
        Vector2 position,
        Dictionary<Vector2, IParticle> particles,
        ParticleKind kind)
    {
        foreach (var offset in _neighborOffsets)
        {
            var neighborPosition = Vector2.Add(position, offset);
            if (particles.TryGetValue(neighborPosition, out IParticle? neighbor) && neighbor.Kind == kind)
            {
                return (neighborPosition, neighbor);
            }
        }
        return null;
    }

    public static string SerializeSimulation(IReadOnlyDictionary<Vector2, IParticle> particles)
    {
        var simulationData = new StringBuilder();

        foreach (var (position, particle) in particles)
        {
            simulationData.AppendLine(
                $"{position.X}|{position.Y}|" +
                $"{particle.Kind}|{particle.Temperature}");
        }

        return simulationData.ToString();
    }
}
