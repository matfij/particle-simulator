using System.Numerics;
using SimulatorEngine;

namespace SimulatorTests;

public class ParticlesGridTest
{
    private readonly List<Particle> TestParticles = new() {
        new SandParticle(new(0, 0)),
        new SandParticle(new(1, 1)),
        new WaterParticle(new(45, 90)),
    };

    [Fact]
    public void Should_GetHashFromPosition()
    {
        var grid = new ParticlesGrid();

        grid.SetParticles(TestParticles);

        var hash = grid.GetHashFromPosition(new(45, 90));

        Assert.Equal(648192, hash);
    }

    [Fact]
    public void Should_GetContentOfCell()
    {
        var grid = new ParticlesGrid();

        grid.SetParticles(TestParticles);
        grid.ClearGrid();
        grid.MapParticlesToCell();

        var hash = grid.GetHashFromPosition(new(0, 0));
        var cellContent = grid.GetContentOfCell(hash);

        Assert.Equal(2, cellContent.Count);
        Assert.Equal(new Vector2(0, 0), cellContent[0].Position);

        var emptyHash = grid.GetHashFromPosition(new(100, 100));
        var emptyCellContent = grid.GetContentOfCell(emptyHash);

        Assert.Empty(emptyCellContent);
    }

    [Fact]
    public void Should_GetNeighborOfParticleIndex()
    {
        var grid = new ParticlesGrid();

        grid.SetParticles(TestParticles);
        grid.ClearGrid();
        grid.MapParticlesToCell();

        var neighbors = grid.GetNeighborOfParticleIndex(0);

        Assert.Equal(2, neighbors.Count);
    }
}
