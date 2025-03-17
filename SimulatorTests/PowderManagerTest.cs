using System.Numerics;
using SimulatorEngine;

namespace SimulatorTests;

public class PowderManagerTest
{

    private readonly float _dt = 1;
    private readonly float _gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleDownWhenNotBlocked()
    {
        var particle = new SandParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [];
        var manager = new PowderManager(_dt, _gravity);

        var position = manager.MovePowder(particle, occupiedPositions);

        Assert.Equal(new(100, 107), position);
    }

    [Fact]
    public void Should_MoveParticleSideDownWhenDownBlocked()
    {
        var particle = new SandParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [new(100, 101)];
        var manager = new PowderManager(_dt, _gravity);

        var position = manager.MovePowder(particle, occupiedPositions);

        Assert.NotEqual(100, position.X);
        Assert.Equal(101, position.Y);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var particle = new SandParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions =
        [
            new(100, 101),
            new(99, 100),
            new(99, 99),
            new(101, 101),
            new(99, 101),
        ];
        var manager = new PowderManager(_dt, _gravity);

        var position = manager.MovePowder(particle, occupiedPositions);

        Assert.Equal(new(100, 100), position);
    }
}
