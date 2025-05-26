using SimulatorEngine;
using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorTests.Managers;

public class ParticleUtilsTest
{
    private readonly Dictionary<Vector2, Particle> _particles = new()
    {
        { new Vector2(99, 100), new IronParticle() },
        { new Vector2(101, 100), new WaterParticle() },
        { new Vector2(100, 99), new WaterParticle() },
        { new Vector2(100, 101), new IronParticle() },
        { new Vector2(99, 99), new LavaParticle() },
        { new Vector2(25, 5), new AcidParticle() },
        { new Vector2(45, 5), new IronParticle() },
        { new Vector2(55, 5), new SaltParticle() }
    };


    [Fact]
    public void Should_GetStrictNeighbors()
    {
        var strictNeighbors = ParticleUtils.GetStrictNeighbors(new Vector2(100, 100), _particles);

        Assert.Equal(
            [
                _particles[new Vector2(100, 101)],
                _particles[new Vector2(99, 100)],
                _particles[new Vector2(101, 100)],
                _particles[new Vector2(100, 99)],
            ],
            strictNeighbors);
    }

    [Fact]
    public void Should_GetNeighbors()
    {
        var neighbors = ParticleUtils.GetNeighbors(new Vector2(100, 100), _particles);

        Assert.Equal(
            [
                _particles[new Vector2(100, 101)],
                _particles[new Vector2(99, 100)],
                _particles[new Vector2(101, 100)],
                _particles[new Vector2(100, 99)],
                _particles[new Vector2(99, 99)],
            ],
            neighbors);
    }

    [Fact]
    public void Should_GetNeighborOfKindIfPresent()
    {
        var neighbor = ParticleUtils.GetNeighborOfKind(new Vector2(100, 100), _particles, ParticleKind.Water);

        Assert.Equal(_particles[new Vector2(101, 100)], neighbor?.Item2);
    }

    [Fact]
    public void Should_NotGetNeighborOfKindIfNotPresent()
    {
        var neighbor = ParticleUtils.GetNeighborOfKind(new Vector2(100, 100), _particles, ParticleKind.Acid);

        Assert.Null(neighbor);
    }
}
