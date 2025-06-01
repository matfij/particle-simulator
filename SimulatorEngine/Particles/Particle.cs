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

public abstract class Particle
{
    public float Temperature { get; set; } = 20;
    public abstract float Density { get; }
    public abstract uint Color { get; }
    public abstract ParticleKind Kind { get; }
    public abstract ParticleBody Body { get; }
    public abstract PhaseTransition[] Transitions { get; }
    public abstract ParticleInteraction[] Interactions { get; set; }
}
