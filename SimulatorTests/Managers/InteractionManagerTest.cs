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

        Assert.Equal(ParticleKind.Water, particles[new Vector2(100, 99)].GetKind());
        Assert.Equal(ParticleKind.Salt, particle.GetKind());

        for (var i = 0; i <= 100; i++)
        {
            updatedParticle = InteractionManager.DoInteractions(position, updatedParticle!, particles);
        }

        Assert.False(particles.TryGetValue(new Vector2(100, 99), out _));
        Assert.Equal(ParticleKind.SaltyWater, updatedParticle?.GetKind());
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

        for (var i = 0; i <= 100; i++)
        {
            if (updatedParticle is null)
            {
                break;
            }
            updatedParticle = InteractionManager.DoInteractions(position, updatedParticle, particles);
        }

        Assert.Null(updatedParticle);
    }
}
