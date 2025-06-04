using System.Numerics;
using SimulatorEngine;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class ParticlesManagerTest
{
    [Fact]
    public void Should_CreateParticleManager()
    {
        var manager = new ParticlesManager();

        Assert.NotNull(manager);
    }

    [Fact]
    public void Should_AddDifferentParticles()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 5), 1, ParticleKind.Sand);
        manager.AddParticles(new(50, 50), 5, ParticleKind.Water);
        manager.AddParticles(new(50, 500), 15, ParticleKind.Iron);
        manager.AddParticles(new(5, 5), 10, ParticleKind.Oxygen);

        Assert.Equal(1008, manager.ParticlesCount);
        Assert.Equal(5, manager.Particles.Where(p => p.Value.Kind == ParticleKind.Sand).Count());
        Assert.Equal(81, manager.Particles.Where(p => p.Value.Kind == ParticleKind.Water).Count());
        Assert.Equal(709, manager.Particles.Where(p => p.Value.Kind == ParticleKind.Iron).Count());
        Assert.Equal(213, manager.Particles.Where(p => p.Value.Kind == ParticleKind.Oxygen).Count());
    }

    [Fact]
    public void Should_AddParticlesInCircularShape()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 2, ParticleKind.Sand);

        Assert.Equal(13, manager.ParticlesCount);
        Assert.Single(manager.Particles, p => p.Key == new Vector2(100, 100));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(100, 99));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(99, 99));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(101, 99));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(101, 101));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(101, 99));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(100, 98));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(100, 101));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(100, 102));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(101, 100));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(102, 100));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(99, 100));
        Assert.Single(manager.Particles, p => p.Key == new Vector2(98, 100));
    }

    [Fact]
    public void Should_NotAddParticlesToSameCoordinates()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 5, ParticleKind.Sand);

        Assert.Equal(81, manager.ParticlesCount);

        manager.AddParticles(new(100, 100), 5, ParticleKind.Sand);
        manager.AddParticles(new(100, 100), 5, ParticleKind.Water);

        Assert.Equal(81, manager.ParticlesCount);
    }

    [Fact]
    public void Should_RemoveAnyParticleKind()
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
    public void Should_NotRemoveParticleOutsideOfRadius()
    {
        var manager = new ParticlesManager();

        manager.AddParticles(new(100, 100), 10, ParticleKind.Sand);

        Assert.Equal(317, manager.ParticlesCount);

        manager.RemoveParticles(new(100, 100), 9);

        Assert.Equal(68, manager.ParticlesCount);
    }

    [Fact]
    public async Task Should_OnlyUpdateParticlesWhenPlaying()
    {
        var manager = new ParticlesManager();

        manager.TogglePlaySimulation(false);

        manager.AddParticles(new(200, 200), 100, ParticleKind.Lava);

        Assert.Equal(0, manager.MoveTime.TotalMilliseconds);
        Assert.Equal(0, manager.InteractionTime.TotalMilliseconds);
        Assert.Equal(0, manager.HeatTransferTime.TotalMilliseconds);

        manager.TogglePlaySimulation(true);

        await Task.Delay(100);

        Assert.NotEqual(0, manager.MoveTime.TotalMilliseconds);
        Assert.NotEqual(0, manager.InteractionTime.TotalMilliseconds);
        Assert.NotEqual(0, manager.HeatTransferTime.TotalMilliseconds);
    }

    [Fact]
    public async Task Should_MoveParticlesBelowTargetTime()
    {
        var manager = new ParticlesManager();

        manager.TogglePlaySimulation(false);

        manager.AddParticles(new(200, 200), 100, ParticleKind.Lava);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Sand);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Oxygen);

        manager.TogglePlaySimulation(true);

        await Task.Delay(1000);

        Assert.InRange(manager.MoveTime.TotalMilliseconds, 1, 400);
    }

    [Fact]
    public async Task Should_DoInteractionsBelowTargetTime()
    {
        var manager = new ParticlesManager();

        manager.TogglePlaySimulation(false);

        manager.AddParticles(new(200, 200), 100, ParticleKind.Water);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Iron);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Oxygen);

        manager.TogglePlaySimulation(true);

        await Task.Delay(100);

        Assert.InRange(manager.InteractionTime.TotalMilliseconds, 10, 50);
    }

    [Fact]
    public async Task Should_TransferHeatBelowTargetTime()
    {
        var manager = new ParticlesManager();

        manager.TogglePlaySimulation(false);

        manager.AddParticles(new(200, 200), 100, ParticleKind.Water);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Iron);
        manager.AddParticles(new(400, 200), 100, ParticleKind.Steam);

        manager.TogglePlaySimulation(true);

        await Task.Delay(1000);

        Assert.InRange(manager.HeatTransferTime.TotalMilliseconds, 1, 200);
    }
}
