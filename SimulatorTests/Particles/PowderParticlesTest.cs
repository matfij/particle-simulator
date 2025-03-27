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
        Assert.Equal(1600, particle.GetDensity());
        Assert.Equal(0xF6D7B0, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Sand, particle.GetKind());
    }

    [Fact]
    public void Should_Create_SaltParticle()
    {
        var particle = new SaltParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(2100, particle.GetDensity());
        Assert.Equal(0xFCF9F3, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Salt, particle.GetKind());
    }
}
