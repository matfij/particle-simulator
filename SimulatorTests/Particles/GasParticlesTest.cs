using SimulatorEngine.Particles;

namespace SimulatorTests.Particles;

public class GasParticlesTest
{
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

