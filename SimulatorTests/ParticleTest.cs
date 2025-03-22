using SimulatorEngine;

namespace SimulatorTests;

public class ParticleTest
{
    [Fact]
    public void Should_Create_SandParticle()
    {
        var particle = new SandParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Powder, particle.Body);
        Assert.Equal(1442, particle.GetDensity());
        Assert.Equal(0xF6D7B0, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Sand, particle.GetKind());
    }

    [Fact]
    public void Should_Create_WaterParticle()
    {
        var particle = new WaterParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(1000, particle.GetDensity());
        Assert.Equal(0x1CA3EC, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Water, particle.GetKind());
    }

    [Fact]
    public void Should_Create_IronParticle()
    {
        var particle = new IronParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Solid, particle.Body);
        Assert.Equal(7800, particle.GetDensity());
        Assert.Equal(0xA19D94, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Iron, particle.GetKind());
    }

    [Fact]
    public void Should_Create_OxygenParticle()
    {
        var particle = new OxygenParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Gas, particle.Body);
        Assert.True(TestUtils.CloseTo(1.4f, particle.GetDensity(), 0.0001f));
        Assert.Equal(0x99E2FA, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Oxygen, particle.GetKind());
    }
}
