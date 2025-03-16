using System.Numerics;
using SimulatorEngine;

namespace SimulatorTests;

public class LiquidManagerTest
{
    private readonly float dt = 1;
    private readonly float gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleDownWhenNotBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [];
        HashSet<Vector2> liquidPositions = [];
        var manager = new LiquidManager(dt, gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, liquidPositions);

        Assert.Equal(new Vector2(100, 105), position);
    }

    [Fact]
    public void Should_MoveParticleSideWhenNotBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [new(100, 101)];
        HashSet<Vector2> liquidPositions = [];
        var manager = new LiquidManager(dt, gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, liquidPositions);

        Assert.NotEqual(100, position.X);
        Assert.InRange(position.Y, 100, 105);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [
            new(99, 100), new(101, 100),
            new(100, 99), new(100, 101),
            new(99, 99), new(99, 101),
            new(99, 101), new(101, 101),
        ];
        HashSet<Vector2> liquidPositions = [];
        var manager = new LiquidManager(dt, gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, liquidPositions);

        Assert.Equal(new Vector2(100, 100), position);
    }

    [Fact]
    public void Should_PassThroughOtherLiquids()
    {
        var particle = new WaterParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [
            new(99, 100), new(101, 100),
            new(100, 99), new(100, 101),
            new(99, 99), new(99, 101),
            new(99, 101), new(101, 101),
        ];
        HashSet<Vector2> liquidPositions = [.. occupiedPositions];
        var manager = new LiquidManager(dt, gravity);

        var position = manager.MoveLiquid(particle, occupiedPositions, liquidPositions);

        Assert.NotEqual(new Vector2(100, 100), position);
    }
}
