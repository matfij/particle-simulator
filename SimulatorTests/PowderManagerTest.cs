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
        var position = new Vector2(100, 100);
        var particle = new SandParticle();
        Dictionary<Vector2, Particle> particles = [];
        var manager = new PowderManager(_dt, _gravity);

        var newPosition = manager.MovePowder(position, particle, particles);

        Assert.Equal(new(100, 107), newPosition);
    }

    [Fact]
    public void Should_MoveParticleSideDownWhenDownBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new SandParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 101), new IronParticle() },
        };
        var manager = new PowderManager(_dt, _gravity);

        var newPosition = manager.MovePowder(position, particle, particles);

        Assert.NotEqual(100, newPosition.X);
        Assert.Equal(101, newPosition.Y);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new SandParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 101), new IronParticle() },
            { new Vector2(99, 101), new IronParticle() },
            { new Vector2(99, 99), new IronParticle() },
            { new Vector2(101, 101), new IronParticle() },
            { new Vector2(101, 100), new IronParticle() },
            { new Vector2(99, 100), new IronParticle() },
        };
        var manager = new PowderManager(_dt, _gravity);

        var newPosition = manager.MovePowder(position, particle, particles);

        Assert.Equal(new(100, 100), newPosition);
    }

    [Fact]
    public void Should_PushLighterParticleUp()
    {
        var position = new Vector2(100, 100);
        var particle = new SandParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 101), new WaterParticle() },
        };
        var manager = new PowderManager(_dt, _gravity);

        Assert.False(particles.ContainsKey(new Vector2(100, 100)));

        var newPosition = manager.MovePowder(position, particle, particles);

        Assert.Equal(new(100, 101), newPosition);
        Assert.True(particles.ContainsKey(new Vector2(100, 100)));
        Assert.IsType<WaterParticle>(particles[new Vector2(100, 100)]);
    }
}
