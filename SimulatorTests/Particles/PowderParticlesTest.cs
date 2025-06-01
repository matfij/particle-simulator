using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class PowderParticlesTest
{
    [Fact]
    public void Should_Create_SandParticle()
    {
        var particle = new SandParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(1600, particle.Density);
        Assert.Equal(0xF6D7B0, (float)particle.Color);
        Assert.Equal(ParticleKind.Sand, particle.Kind);
    }

    [Fact]
    public void Should_Create_SaltParticle()
    {
        var particle = new SaltParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(2100, particle.Density);
        Assert.Equal(0xFCF9F3, (float)particle.Color);
        Assert.Equal(ParticleKind.Salt, particle.Kind);
    }

    [Fact]
    public void Should_Create_StoneParticle()
    {
        var particle = new StoneParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(2500, particle.Density);
        Assert.Equal(0x787A79, (float)particle.Color);
        Assert.Equal(ParticleKind.Stone, particle.Kind);
    }
}
