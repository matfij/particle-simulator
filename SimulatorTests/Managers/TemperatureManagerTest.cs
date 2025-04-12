using System.Numerics;
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
        Assert.Equal(1200, lavaParticle1.Temperature);
        Assert.Equal(1200, lavaParticle2.Temperature);

        TemperatureManager.TransferHeat(particles);

        Assert.InRange(waterParticle.Temperature, 50, 100);
        Assert.InRange(lavaParticle1.Temperature, 1100, 1190);
        Assert.InRange(lavaParticle2.Temperature, 1100, 1190);
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
}
