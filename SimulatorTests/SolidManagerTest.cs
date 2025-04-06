using System.Numerics;
using SimulatorEngine;
using SimulatorEngine.Particles;

namespace SimulatorTests;

public class SolidManagerTest
{
    [Fact]
    public void Should_Dissolve_Iron_Near_Acid()
    {
        var position = new Vector2(100, 100);
        Particle? particle = new IronParticle();
        Dictionary<Vector2, Particle> particles = new()
        {
            {  new Vector2(100, 101), new AcidParticle() }
        };

        for (int i = 0; i < 2; i++)
        {
            particle = SolidManager.DoInteractions(position, particle!, particles);
        }

        Assert.NotNull(particle);
        Assert.IsType<IronParticle>(particle);
        Assert.Equal(3, ((IronParticle)particle).TicksToDissolveInAcid);

        for (int i = 0; i < 3; i++)
        {
            particle = SolidManager.DoInteractions(position, particle!, particles);
        }

        Assert.Null(particle);
    }
}
