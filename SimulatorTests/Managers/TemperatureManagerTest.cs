﻿using System.Numerics;
using SimulatorEngine.Managers;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class TemperatureManagerTest
{
    [Fact]
    public void Should_TransferHeatToColderParticle()
    {
        var waterParticle = new WaterParticle();
        var lavaParticle1 = new LavaParticle();
        var lavaParticle2 = new LavaParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 100), waterParticle },
            { new Vector2(100, 99), lavaParticle1 },
            { new Vector2(100, 101), lavaParticle2 },
        };

        Assert.Equal(20, waterParticle.Temperature);
        Assert.Equal(1600, lavaParticle1.Temperature);
        Assert.Equal(1600, lavaParticle2.Temperature);

        TemperatureManager.TransferHeat(particles);

        Assert.InRange(waterParticle.Temperature, 150, 200);
        Assert.InRange(lavaParticle1.Temperature, 1500, 1550);
        Assert.InRange(lavaParticle2.Temperature, 1500, 1550);
    }

    [Fact]
    public void ShouldNot_TransferHeatBelowTransferThreshold()
    {
        var waterParticle1 = new WaterParticle();
        var waterParticle2 = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 100), waterParticle1 },
            { new Vector2(100, 101), waterParticle2 },
        };

        waterParticle1.Temperature = 20.07f;
        waterParticle2.Temperature = 20.05f;

        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);

        Assert.True(TestUtils.CloseTo(20.07f, waterParticle1.Temperature));
        Assert.True(TestUtils.CloseTo(20.05f, waterParticle2.Temperature));
    }

    [Fact]
    public void ShouldNot_TransferHeatWhenNotAdjacent()
    {
        var waterParticle1 = new WaterParticle();
        var waterParticle2 = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 100), waterParticle1 },
            { new Vector2(100, 250), waterParticle2 },
        };

        waterParticle1.Temperature = 100;
        waterParticle2.Temperature = 250;

        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);

        Assert.True(TestUtils.CloseTo(100f, waterParticle1.Temperature));
        Assert.True(TestUtils.CloseTo(250f, waterParticle2.Temperature));
    }

    [Fact]
    public void ShouldNot_ShiftPhaseWhenTargetTemperatureNotReached()
    {
        var waterParticle = new WaterParticle();
        var sandParticle = new SandParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 100), waterParticle },
            { new Vector2(100, 101), sandParticle },
        };

        sandParticle.Temperature = 90;

        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);
        TemperatureManager.TransferHeat(particles);

        Assert.Equal(ParticleKind.Water, waterParticle.Kind);
        Assert.Equal(ParticleKind.Sand, sandParticle.Kind);
    }

    [Fact]
    public void Should_ShiftPhaseUpWhenTargetTemperatureReached()
    {
        var waterParticle = new WaterParticle();
        var lavaParticle = new LavaParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 101), waterParticle },
            { new Vector2(100, 100), lavaParticle },
        };

        for (var i = 0; i < 10; i++)
        {
            TemperatureManager.TransferHeat(particles);
        }

        Assert.Equal(ParticleKind.Steam, particles[new Vector2(100, 101)].Kind);
    }

    [Fact]
    public void Should_ShiftPhaseDownWhenTargetTemperatureReached()
    {
        var steamParticle = new SteamParticle();
        var ironParticle = new IronParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 100), steamParticle },
            { new Vector2(100, 101), ironParticle },
        };

        ironParticle.Temperature = -200;

        for (var i = 0; i < 100; i++)
        {
            TemperatureManager.TransferHeat(particles);
        }

        Assert.Equal(ParticleKind.Water, particles[new Vector2(100, 100)].Kind);
    }
}
