using System.Numerics;
using SimulatorEngine;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class ParticlesManagerTest
{
    [Fact]
    public void Should_Create_ParticleManager()
    {
        var manager = new ParticlesManager();

        Assert.NotNull(manager);
    }

    [Fact]
    public void Should_Add_Different_Particles()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 5), 1, ParticleKind.Sand);
        manager.AddParticles(new(50, 50), 5, ParticleKind.Water);
        manager.AddParticles(new(50, 500), 15, ParticleKind.Iron);
        manager.AddParticles(new(5, 5), 10, ParticleKind.Oxygen);

        Assert.Equal(1008, manager.ParticlesCount);
        Assert.Equal(5, manager.Particles.Where(p => p.Value.GetKind() == ParticleKind.Sand).Count());
        Assert.Equal(81, manager.Particles.Where(p => p.Value.GetKind() == ParticleKind.Water).Count());
        Assert.Equal(709, manager.Particles.Where(p => p.Value.GetKind() == ParticleKind.Iron).Count());
        Assert.Equal(213, manager.Particles.Where(p => p.Value.GetKind() == ParticleKind.Oxygen).Count());
    }

    [Fact]
    public void Should_Add_Particles_In_Circular_Shape()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 2, ParticleKind.Sand);

        Assert.Equal(13, manager.ParticlesCount);
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(100, 100)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(100, 99)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(99, 99)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(101, 99)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(101, 101)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(101, 99)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(100, 98)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(100, 101)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(100, 102)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(101, 100)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(102, 100)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(99, 100)));
        Assert.Single(manager.Particles.Where(p => p.Key == new Vector2(98, 100)));
    }

    [Fact]
    public void Should_Not_Add_Particles_To_Same_Coordinates()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 5, ParticleKind.Sand);

        Assert.Equal(81, manager.ParticlesCount);

        manager.AddParticles(new(100, 100), 5, ParticleKind.Sand);
        manager.AddParticles(new(100, 100), 5, ParticleKind.Water);

        Assert.Equal(81, manager.ParticlesCount);
    }

    [Fact]
    public void Should_Remove_Any_Particle_Kind()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(10, 10), 3, ParticleKind.Sand);
        manager.AddParticles(new(10, 100), 5, ParticleKind.Iron);
        manager.AddParticles(new(500, 100), 10, ParticleKind.Oxygen);

        Assert.Equal(427, manager.ParticlesCount);

        manager.RemoveParticles(new(10, 10), 4);

        Assert.Equal(398, manager.ParticlesCount);

        manager.RemoveParticles(new(10, 100), 6);

        Assert.Equal(317, manager.ParticlesCount);

        manager.RemoveParticles(new(500, 100), 11);

        Assert.Equal(0, manager.ParticlesCount);
    }

    [Fact]
    public void Should_Not_Remove_Particle_Outside_Of_Radius()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 10, ParticleKind.Sand);

        Assert.Equal(317, manager.ParticlesCount);

        manager.RemoveParticles(new(100, 100), 9);

        Assert.Equal(68, manager.ParticlesCount);
    }
}
