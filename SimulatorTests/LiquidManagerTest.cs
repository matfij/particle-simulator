using System.Numerics;
using SimulatorEngine;

namespace SimulatorTests;

public class LiquidManagerTest
{
    private float dt = 1;
    private float gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleDownWhenNotBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [];
        var manager = new LiquidManager(gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, dt);

        Assert.Equal(new Vector2(100, 105), position);
    }

    [Fact]
    public void Should_MoveParticleSideWhenNotBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [new(100, 101)];
        var manager = new LiquidManager(gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, dt);

        Assert.NotEqual(100, position.X);
        Assert.InRange(position.Y, 100, 105);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [new(99, 100), new(101, 100), new(100, 99), new(100, 101)];
        var manager = new LiquidManager(gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, dt);

        Assert.Equal(new Vector2(100, 105), position);
    }
}
