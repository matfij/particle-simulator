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
    Plant,
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

    protected uint ComputeTemperatureAdjustedColor(int baseRed, int green, int blue, float shiftFactor)
    {
        int redShift = (int)(Temperature / shiftFactor);
        int red = baseRed + redShift;
        if (red > 255)
        {
            red = 255;
        }
        else if (red < 0)
        {
            red = 0;
        }
        return (uint)((red << 16) | (green << 8) | blue);
    }
}
