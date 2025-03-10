﻿using System.Numerics;

namespace SimulatorEngine;

public class ParticlesGrid
{
    private const int cellSize = 20;
    private const long PrimeX = 6614058611;
    private const long PrimeY = 7528850467;
    private const long HashMapSize = 1000000;
    private readonly Dictionary<long, List<Particle>> hashMap = [];
    private List<Particle> Particles = [];

    public List<Particle> GetContentOfCell(long id) => hashMap.TryGetValue(id, out var particles) ? particles : [];

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

            if (hashMap.TryGetValue(hash, out var particles))
            {
                particles.Add(particle);
            }
            else
            {
                hashMap[hash] = [particle];
            }
        }
    }

    public List<Particle> GetNeighborOfParticleIndex(int index)
    {
        if (Particles.Count < index)
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
                var hash = CellIndexToHash(gridX, gridY);
                var content = GetContentOfCell(hash);
                foreach (var particle in content)
                {
                    neighbors.Add(particle);
                }
            }
        }

        return neighbors;
    }

    private static long CellIndexToHash(int x, int y) => (x * PrimeX + y * PrimeY) % HashMapSize;
}
