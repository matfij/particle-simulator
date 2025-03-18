using System.Numerics;
using SimulatorEngine;

namespace SimulatorTests;

public class GasManagerTest
{
    private readonly float _dt = 1;
    private readonly float _gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleWhenNotBlocked()
    {
        var particle = new OxygenParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions = [];
        var manager = new GasManager(_dt, _gravity);

        var position = manager.MoveGas(particle, occupiedPositions);

        Assert.NotEqual(particle.Position, position);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var particle = new OxygenParticle(new(100, 100));
        HashSet<Vector2> occupiedPositions =
        [
            new(99, 100),
            new(101, 100),
            new(100, 99),
            new(100, 101),
            new(99, 99),
            new(99, 101),
            new(101, 99),
            new(101, 101),
        ];
        var manager = new GasManager(_dt, _gravity);

        var position = manager.MoveGas(particle, occupiedPositions);

        Assert.Equal(particle.Position, position);
    }
}
