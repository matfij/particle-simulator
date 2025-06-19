using System.Numerics;
using SimulatorEngine.Managers;
using SimulatorEngine.Particles;

namespace SimulatorTests.Managers;

public class InteractionManagerTest
{
    [Fact]
    public void Should_DoMergeInteraction()
    {
        var position = new Vector2(100, 100);
        var particle = new SaltParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(100, 99), new WaterParticle() },
        };

        var updatedParticle = InteractionManager.DoInteractions(position, particle, particles);

        Assert.NotNull(updatedParticle);
        Assert.Equal(ParticleKind.Water, particles[new Vector2(100, 99)].Kind);
        Assert.Equal(ParticleKind.Salt, particle.Kind);

        var waterInteractionTicks = updatedParticle.Interactions.First(i => i.NeighborKind == ParticleKind.Water).Ticks;
        for (var i = 0; i <= waterInteractionTicks; i++)
        {
            updatedParticle = InteractionManager.DoInteractions(position, updatedParticle!, particles);
        }

        Assert.False(particles.TryGetValue(new Vector2(100, 99), out _));
        Assert.Equal(ParticleKind.SaltyWater, updatedParticle?.Kind);
    }

    [Fact]
    public void Should_DoRemoveSelfInteraction()
    {
        var position = new Vector2(256, 256);
        var particle = new IronParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { new Vector2(255, 256), new AcidParticle() }
        };

        var updatedParticle = InteractionManager.DoInteractions(position, particle, particles);

        Assert.NotNull(updatedParticle);
        Assert.Equal(particle.Kind, updatedParticle.Kind);

        var acidInteractionTicks = updatedParticle.Interactions.First(i => i.NeighborKind == ParticleKind.Acid).Ticks;
        for (var i = 0; i <= acidInteractionTicks; i++)
        {
            if (updatedParticle is null)
            {
                break;
            }
            updatedParticle = InteractionManager.DoInteractions(position, updatedParticle, particles);
        }

        Assert.Null(updatedParticle);
        Assert.True(particles.TryGetValue(new Vector2(255, 256), out var acidParticle));
        Assert.Equal(ParticleKind.Acid, acidParticle.Kind);
    }

    [Fact]
    public void Should_DoExpireTransformInteraction()
    {
        var position = new Vector2(128, 128);
        var particle = new FireParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            { position,  particle },
        };

        var updatedParticle = InteractionManager.DoInteractions(position, particle, particles);

        Assert.Equal(ParticleKind.Fire, updatedParticle?.Kind);
        
        for (var i = 0; i <= 100; i++)
        {
            if (updatedParticle is null)
            {
                break;
            }
            updatedParticle = InteractionManager.DoInteractions(position, updatedParticle, particles);
        }

        Assert.Equal(ParticleKind.Smoke, updatedParticle?.Kind);
    }
}
