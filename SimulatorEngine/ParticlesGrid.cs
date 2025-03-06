using System.Numerics;

namespace SimulatorEngine;

public class ParticlesGrid
{
    private const int cellSize = 10;
    private const long PrimeX = 6614058611;
    private const long PrimeY = 7528850467;
    private const long HashMapSize = 1000000;
    private readonly Dictionary<long, List<Particle>> hashMap = new();
    private List<Particle> Particles = new();

    public List<Particle> GetContentOfCell(long id) => hashMap.ContainsKey(id) ? hashMap[id] : [];

    public void SetParticles(List<Particle> particles)
    {
        Particles = particles;
    }

    public void ClearGrid()
    {
        hashMap.Clear();
    }

    public long GetHashFromPosition(Vector2 position)
    {
        int x = (int)(position.X / cellSize);
        int y = (int)(position.Y / cellSize);
        return CellIndexToHash(x, y);
    }

    public void MapParticlesToCell()
    {
        foreach (var particle in Particles)
        {
            var position = particle.Position;
            int x = (int)(position.X / cellSize);
            int y = (int)(position.Y / cellSize);
            long hash = CellIndexToHash(x, y);

            if (!hashMap.ContainsKey(hash))
            {
                hashMap[hash] = new List<Particle>();
            }
            hashMap[hash].Add(particle);
        }
    }

    public List<Particle> GetNeighborOfParticleIndex(int index)
    {
        if (Particles.Count < index || index < 0)
        {
            return [];
        }

        var neighbors = new List<Particle>();
        var position = Particles[index].Position;

        int particleGridX = (int)(position.X / cellSize);
        int particleGridY = (int)(position.Y / cellSize);

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var gridX = particleGridX + x;
                var gridY = particleGridY + y;
                var hash = GetHashFromPosition(new(gridX, gridY));
                var content = GetContentOfCell(hash);
                foreach (var particle in content)
                {
                    neighbors.Add(particle);
                }
            }
        }

        return neighbors;
    }

    // TODO - cash hash
    // private readonly Dictionary<Vector2, long> hashCache = new();
    private long CellIndexToHash(int x, int y) => (x * PrimeX + y * PrimeY) % HashMapSize;
}
