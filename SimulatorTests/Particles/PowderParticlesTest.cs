using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class PowderParticlesTest
{
    [Fact]
    public void Should_CreateSandParticle()
    {
        var particle = new SandParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(1600, particle.Density);
        Assert.Equal(0xF8D7B0, (float)particle.Color);
        Assert.Equal(ParticleKind.Sand, particle.Kind);
    }

    [Fact]
    public void Should_AdjustSandParticleColorBasedOnTemperature()
    {
        var particle = new SandParticle();

        Assert.Equal(0xF8D7B0, (float)particle.Color);

        particle.Temperature = 500f;

        Assert.Equal(0xFFD7B0, (float)particle.Color);
    }

    [Fact]
    public void Should_CreateSaltParticle()
    {
        var particle = new SaltParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(2100, particle.Density);
        Assert.Equal(0xFEF9F3, (float)particle.Color);
        Assert.Equal(ParticleKind.Salt, particle.Kind);
    }

    [Fact]
    public void Should_AdjustSaltParticleColorBasedOnTemperature()
    {
        var particle = new SaltParticle();

        Assert.Equal(0xFEF9F3, (float)particle.Color);

        particle.Temperature = 250f;

        Assert.Equal(0xFFF9F3, (float)particle.Color);
    }

    [Fact]
    public void Should_CreateStoneParticle()
    {
        var particle = new StoneParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(2500, particle.Density);
        Assert.Equal(0x7B7A79, (float)particle.Color);
        Assert.Equal(ParticleKind.Stone, particle.Kind);
    }

    [Fact]
    public void Should_AdjustStoneParticleColorBasedOnTemperature()
    {
        var particle = new StoneParticle();

        Assert.Equal(0x7B7A79, (float)particle.Color);

        particle.Temperature = 800f;

        Assert.Equal(0xFD7A79, (float)particle.Color);
    }
}
