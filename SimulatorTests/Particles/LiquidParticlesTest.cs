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
        Assert.Equal(1000, particle.Density);
        Assert.Equal(0x1CA3EC, (float)particle.Color);
        Assert.Equal(ParticleKind.Water, particle.Kind);
    }

    [Fact]
    public void Should_Create_SaltyWaterParticle()
    {
        var particle = new SaltyWaterParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(1025, particle.Density);
        Assert.Equal(0x90AEBD, (float)particle.Color);
        Assert.Equal(ParticleKind.SaltyWater, particle.Kind);
    }

    [Fact]
    public void Should_Create_AcidParticle()
    {
        var particle = new AcidParticle();

        Assert.NotNull(particle);
        Assert.Equal(300, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(1100, particle.Density);
        Assert.Equal(0x89FF00, (float)particle.Color);
        Assert.Equal(ParticleKind.Acid, particle.Kind);
    }

    [Fact]
    public void Should_Create_LavaParticle()
    {
        var particle = new LavaParticle();

        Assert.NotNull(particle);
        Assert.Equal(1600, particle.Temperature);
        Assert.Equal(ParticleBody.Liquid, particle.Body);
        Assert.Equal(2200, particle.Density);
        Assert.Equal(0xCF1020, (float)particle.Color);
        Assert.Equal(ParticleKind.Lava, particle.Kind);
    }
}
