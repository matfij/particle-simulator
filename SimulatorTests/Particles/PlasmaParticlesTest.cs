using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class PlasmaParticlesTest
{

    [Fact]
    public void Should_CreateFireParticle()
    {
        var particle = new FireParticle();

        Assert.NotNull(particle);
        Assert.Equal(500, particle.Temperature);
        Assert.Equal(ParticleBody.Plasma, particle.Body);
        Assert.True(TestUtils.CloseTo(0.45f, particle.Density));
        Assert.Equal(0xFF4412, (float)particle.Color);
        Assert.Equal(ParticleKind.Fire, particle.Kind);
    }

    [Fact]
    public void Should_CreateSmokeParticle()
    {
        var particle = new SmokeParticle();

        Assert.NotNull(particle);
        Assert.Equal(150, particle.Temperature);
        Assert.Equal(ParticleBody.Plasma, particle.Body);
        Assert.True(TestUtils.CloseTo(0.3f, particle.Density));
        Assert.Equal(0x848884, (float)particle.Color);
        Assert.Equal(ParticleKind.Smoke, particle.Kind);
    }

    [Fact]
    public void Should_CreatePlasmaParticle()
    {
        var particle = new PlasmaParticle();

        Assert.NotNull(particle);
        Assert.Equal(6000, particle.Temperature);
        Assert.Equal(ParticleBody.Plasma, particle.Body);
        Assert.True(TestUtils.CloseTo(0.4f, particle.Density));
        Assert.Equal(0xFF41CA, (float)particle.Color);
        Assert.Equal(ParticleKind.Plasma, particle.Kind);
    }
}
