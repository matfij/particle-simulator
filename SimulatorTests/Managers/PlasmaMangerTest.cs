using System.Numerics;
using SimulatorEngine.Managers;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class PlasmaMangerTest
{
    private readonly float _dt = 1;
    private readonly float _gravity = 0.005f;

    [Fact]
    public void Should_MoveParticleWhenNotBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = [];
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.NotEqual(new Vector2(100, 100), newPosition);
    }

    [Fact]
    public void Should_NotMoveParticleWhenBlocked()
    {
        var position = new Vector2(100, 100);
        var particle = new WaterParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(99, 100), new IronParticle() },
            { new Vector2(99, 101), new IronParticle() },
            { new Vector2(100, 99), new IronParticle() },
            { new Vector2(100, 101), new IronParticle() },
            { new Vector2(101, 100), new IronParticle() },
        };
        var manager = new LiquidManager(_dt, _gravity);

        var newPosition = manager.MoveLiquid(position, particle, particles);

        Assert.Equal(new Vector2(100, 100), newPosition);
    }
}
