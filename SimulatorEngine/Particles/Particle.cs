namespace SimulatorEngine.Particles;

public enum ParticleKind
{
    None,
    Sand,
    Water,
    Iron,
    Oxygen,
    Salt,
    SaltyWater,
    Acid,
    Lava,
    Stone,
    Steam,
}

public enum ParticleBody
{
    Solid,
    Powder,
    Liquid,
    Gas,
}

public abstract class ParticleData
{
    public abstract ParticleBody Body { get; init; }
    public abstract float Density { get; init; }
    public abstract uint Color { get; init; }
    public virtual List<PhaseTransition> Transitions { get; init; } = [];
    public virtual List<ParticleInteraction> Interactions { get; init; } = [];
}

public interface IParticle
{
    public ParticleKind Kind { get; init; }
    public float Temperature { get; set; }
}
