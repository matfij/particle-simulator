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
        Assert.InRange(lavaParticle1.Temperature, 1100, 1180);
        Assert.InRange(lavaParticle2.Temperature, 1100, 1180);
    }
}
