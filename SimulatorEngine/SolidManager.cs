using SimulatorEngine.Particles;
using System.Numerics;

namespace SimulatorEngine;

public class SolidManager
{
    public static Particle? DoInteractions(Vector2 position, Particle particle, Dictionary<Vector2, Particle> particles)
    {
        var dissolvedInAcid = false;

        var neighbors = ParticleUtils.GetNeighbors(position, particles);
        
        foreach (var neighbor in neighbors)
        {
            if (!dissolvedInAcid && particle is IronParticle ironParticle && neighbor is AcidParticle)
            {
                ironParticle.TicksToDissolveInAcid -= 1;
                dissolvedInAcid = true;
                if (ironParticle.TicksToDissolveInAcid <= 0)
                {
                    return null;
                }
            }
        }

        return particle;
    }
}
