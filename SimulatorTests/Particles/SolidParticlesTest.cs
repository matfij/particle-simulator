using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class SolidParticlesTest
{
    [Fact]
    public void Should_Create_IronParticle()
    {
        var particle = new IronParticle();

        Assert.NotNull(particle);
        Assert.Equal(20, particle.Temperature);
        Assert.Equal(ParticleBody.Solid, particle.Body);
        Assert.Equal(7800, particle.Density);
        Assert.Equal(0xA19D94, (float)particle.Color);
        Assert.Equal(ParticleKind.Iron, particle.Kind);
    }
}
