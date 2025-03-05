using SimulatorEngine;

namespace SimulatorTests;

public class ParticlesGridTest
{
    private List<Particle> TestParticles = new() {
        new SandParticle(new(0, 0)),
        new SandParticle(new(5, 5)),
        new WaterParticle(new(45, 90)),
    };

    [Fact]
    public void Should_GetHashFromPosition()
    {
        var grid = new ParticlesGrid();

        grid.SetParticles(TestParticles);

        var hash = grid.GetHashFromPosition(new(45, 90));

        Assert.Equal(888647, hash);
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
