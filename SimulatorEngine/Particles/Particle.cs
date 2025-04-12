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
    public ParticleBody Body { get; set; }
    public List<ParticleInteraction> Interactions { get; set; }

    protected Particle()
    {
        Temperature = 20;
        Interactions = [];
    }

    public abstract ParticleKind GetKind();

    public abstract uint GetColor();

    public abstract float GetDensity();
}
