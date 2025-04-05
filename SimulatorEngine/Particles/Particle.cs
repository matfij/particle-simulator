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
}

public enum ParticleBody
{
    Solid,
    Powder,
    Liquid,
    Gas,
}

public abstract class Particle : IComparable<Particle>
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

    public int CompareTo(Particle? other)
    {
        if (other?.GetDensity() > GetDensity())
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
