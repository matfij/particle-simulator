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
        var position = new Vector2(100, 100);
        var particle = new OxygenParticle();
        Dictionary<Vector2, Particle> particles = [];
        var manager = new GasManager(_dt, _gravity);

        var newPosition = manager.MoveGas(position, particle, particles);

        Assert.NotEqual(newPosition, position);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new OxygenParticle();
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
        var manager = new GasManager(_dt, _gravity);

        var newPosition = manager.MoveGas(position, particle, particles);

        Assert.Equal(newPosition, position);
    }
}
