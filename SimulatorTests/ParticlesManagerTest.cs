using SimulatorEngine;

namespace SimulatorTests;

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

        manager.AddParticles((100, 5), 1, ParticleKind.Sand);
        manager.AddParticles((50, 50), 5, ParticleKind.Water);
        manager.AddParticles((50, 500), 15, ParticleKind.Iron);
        manager.AddParticles((5, 5), 10, ParticleKind.Oxygen);

        Assert.Equal(1112, manager.GetParticles.Count());
        Assert.Equal(5, manager.GetParticles.Where(p => p.GetKind() == ParticleKind.Sand).Count());
        Assert.Equal(81, manager.GetParticles.Where(p => p.GetKind() == ParticleKind.Water).Count());
        Assert.Equal(709, manager.GetParticles.Where(p => p.GetKind() == ParticleKind.Iron).Count());
        Assert.Equal(317, manager.GetParticles.Where(p => p.GetKind() == ParticleKind.Oxygen).Count());
    }

    [Fact]
    public void Should_Add_Particles_In_Circular_Shape()
    {
        var manager = new ParticlesManager();

        manager.AddParticles((100, 100), 2, ParticleKind.Sand);

        Assert.Equal(13, manager.GetParticles.Count());
        Assert.Single(manager.GetParticles.Where(p => p.X == 100 && p.Y == 100));
        Assert.Single(manager.GetParticles.Where(p => p.X == 100 && p.Y == 99));
        Assert.Single(manager.GetParticles.Where(p => p.X == 99 && p.Y == 99));
        Assert.Single(manager.GetParticles.Where(p => p.X == 101 && p.Y == 99));
        Assert.Single(manager.GetParticles.Where(p => p.X == 101 && p.Y == 101));
        Assert.Single(manager.GetParticles.Where(p => p.X == 101 && p.Y == 99));
        Assert.Single(manager.GetParticles.Where(p => p.X == 100 && p.Y == 98));
        Assert.Single(manager.GetParticles.Where(p => p.X == 100 && p.Y == 101));
        Assert.Single(manager.GetParticles.Where(p => p.X == 100 && p.Y == 102));
        Assert.Single(manager.GetParticles.Where(p => p.X == 101 && p.Y == 100));
        Assert.Single(manager.GetParticles.Where(p => p.X == 102 && p.Y == 100));
        Assert.Single(manager.GetParticles.Where(p => p.X == 99 && p.Y == 100));
        Assert.Single(manager.GetParticles.Where(p => p.X == 98 && p.Y == 100));
    }
}
