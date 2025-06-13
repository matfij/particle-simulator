﻿using SimulatorEngine.Particles;

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
        Assert.True(TestUtils.CloseTo(1.4f, particle.Density));
        Assert.Equal(0x99E2FA, (float)particle.Color);
        Assert.Equal(ParticleKind.Oxygen, particle.Kind);
    }

    [Fact]
    public void Should_Create_SteamParticle()
    {
        var particle = new SteamParticle();

        Assert.NotNull(particle);
        Assert.Equal(128, particle.Temperature);
        Assert.Equal(ParticleBody.Gas, particle.Body);
        Assert.True(TestUtils.CloseTo(15f, particle.Density));
        Assert.Equal(0xC7D5E0, (float)particle.Color);
        Assert.Equal(ParticleKind.Steam, particle.Kind);
    }
}
