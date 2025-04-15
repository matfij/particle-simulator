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
    public float Temperature { get; set; }
    public ParticleBody Body { get; init; }
    public List<PhaseTransition> Transitions { get; init; }
    public List<ParticleInteraction> Interactions { get; init; }

    protected Particle()
    {
        Temperature = 20;
        Transitions = [];
        Interactions = [];
    }

    public abstract ParticleKind GetKind();

    public abstract uint GetColor();

    public abstract float GetDensity();
}
