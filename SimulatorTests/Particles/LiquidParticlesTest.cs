using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class LiquidParticlesTest
{
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
    public void Should_Create_SaltyWaterParticle()
    {
        var particle = new SaltyWaterParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(1025, particle.GetDensity());
        Assert.Equal(0x90AEBD, (float)particle.GetColor());
        Assert.Equal(ParticleKind.SaltyWater, particle.GetKind());
    }

    [Fact]
    public void Should_Create_AcidParticle()
    {
        var particle = new AcidParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(1100, particle.GetDensity());
        Assert.Equal(0x89FF00, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Acid, particle.GetKind());
    }

    [Fact]
    public void Should_Create_LavaParticle()
    {
        var particle = new LavaParticle();

        Assert.NotNull(particle);
        Assert.Equal(1200, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(2200, particle.GetDensity());
        Assert.Equal(0xCF1020, (float)particle.GetColor());
        Assert.Equal(ParticleKind.Lava, particle.GetKind());
    }
}
