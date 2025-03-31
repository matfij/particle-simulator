using System.Numerics;
using SimulatorEngine;
using SimulatorEngine.Particles;

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

        Assert.Equal(new(100, 108), newPosition);
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
    public void Should_PushUpLighterParticle()
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

    [Fact]
    public void Should_DissolveSaltInWater()
    {
        var position = new Vector2(100, 100);
        var particle = new SaltParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 99), new WaterParticle() },
            { new Vector2(100, 101), new IronParticle() },
            { new Vector2(98, 101), new IronParticle() },
            { new Vector2(97, 101), new IronParticle() },
            { new Vector2(96, 101), new IronParticle() },
            { new Vector2(99, 101), new IronParticle() },
            { new Vector2(101, 101), new IronParticle() },
            { new Vector2(101, 100), new IronParticle() },
            { new Vector2(99, 100), new IronParticle() },
            { new Vector2(99, 99), new IronParticle() },
            { new Vector2(99, 98), new IronParticle() },
            { new Vector2(99, 97), new IronParticle() },
            { new Vector2(99, 96), new IronParticle() },
        };
        var manager = new PowderManager(_dt, _gravity);

        var p = manager.MovePowder(position, particle, particles);

        Assert.Equal(ParticleKind.Water, particles[new Vector2(100, 99)].GetKind());
        Assert.Equal(ParticleKind.Salt, particle.GetKind());

        for (var i = 0; i <= 100;  i++)
        {
            p = manager.MovePowder(position, particle, particles);
        }

        Assert.Equal(ParticleKind.SaltyWater, particles[new Vector2(100, 100)].GetKind());
        Assert.Equal(ParticleKind.Salt, particle.GetKind());
    }
}
