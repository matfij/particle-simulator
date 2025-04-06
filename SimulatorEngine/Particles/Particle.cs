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
    public int Temperature { get; set; }
    public ParticleBody Body { get; set; }

    protected Particle()
    {
        Temperature = 20;
    }

    public abstract ParticleKind GetKind();

    public abstract uint GetColor();

    public abstract float GetDensity();
}
