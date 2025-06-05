using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class SolidParticlesTest
{
    [Fact]
    public void Should_CreateIronParticle()
    {
        var particle = new IronParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Solid, particle.Body);
        Assert.Equal(7800, particle.Density);
        Assert.Equal(0xA49D94, (float)particle.Color);
        Assert.Equal(ParticleKind.Iron, particle.Kind);
    }

    [Fact]
    public void Should_AdjustIronParticleColorBasedOnTemperature()
    {
        var particle = new IronParticle();

        Assert.Equal(0xA49D94, (float)particle.Color);

        particle.Temperature = 1000f;

        Assert.Equal(0xFF9D94, (float)particle.Color);
    }

    [Fact]
    public void Should_CreatePlantParticle()
    {
        var particle = new PlantParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Solid, particle.Body);
        Assert.Equal(400, particle.Density);
        Assert.Equal(0x4CD038, (float)particle.Color);
        Assert.Equal(ParticleKind.Plant, particle.Kind);
    }
}
