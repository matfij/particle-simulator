using System.Numerics;
using SimulatorEngine.Managers;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class LiquidManagerTest
{
    private readonly float _dt = 1;
    private readonly float _gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleDownWhenNotBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = [];
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.Equal(105, newPosition.Y);
    }

    [Fact]
    public void Should_MoveParticleSideWhenNotBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 101), new IronParticle() },
        };
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.InRange(newPosition.Y, 102, 105);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(99, 100), new IronParticle() },
            { new Vector2(101, 100), new IronParticle() },
            { new Vector2(100, 99), new IronParticle() },
            { new Vector2(100, 101), new IronParticle() },
            { new Vector2(99, 99), new IronParticle() },
            { new Vector2(99, 101), new IronParticle() },
            { new Vector2(101, 99), new IronParticle() },
            { new Vector2(101, 101), new IronParticle() }
        };
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.Equal(new Vector2(100, 100), position);
    }

    [Fact]
    public void Should_PassThroughOtherLiquids()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(99, 100), new WaterParticle() },
            { new Vector2(101, 100), new WaterParticle() },
            { new Vector2(100, 99), new WaterParticle() },
            { new Vector2(100, 101), new WaterParticle() },
            { new Vector2(99, 99), new WaterParticle() },
            { new Vector2(99, 101), new WaterParticle() },
            { new Vector2(101, 99), new WaterParticle() },
            { new Vector2(101, 101), new WaterParticle() }
        };
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.NotEqual(new Vector2(100, 100), newPosition);
    }
}
