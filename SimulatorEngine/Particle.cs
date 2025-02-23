namespace SimulatorEngine;

public enum ParticleBody
{
    Solid,
    Powder,
    Liquid,
    Gas,
}

public abstract class Particle(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int Temperature { get; set; }
    public ParticleBody Body { get; set; }

    public abstract uint GetColor();

    public abstract uint GetDensity();
}

public class SandParticle : Particle
{
    private static readonly uint Density = 1442;
    private static readonly uint Color = 0xEFEBF01;

    public SandParticle(int x, int y) : base(x, y)
    {
        Temperature = 20;
        Body = ParticleBody.Powder;
    }

    public override uint GetColor() => Color;

    public override uint GetDensity() => Density;
}
