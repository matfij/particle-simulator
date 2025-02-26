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

    public abstract float GetDensity();
}

public class SandParticle : Particle
{
    private static readonly float Density = 1442f;
    private static readonly uint Color = 0xEFEBF01;

    public SandParticle(int x, int y) : base(x, y)
    {
        Temperature = 20;
        Body = ParticleBody.Powder;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;
}

public class WaterParticle : Particle
{
    private static readonly float Density = 1000f;
    private static readonly uint Color = 0x1CA3EC;

    public WaterParticle(int x, int y) : base(x, y)
    {
        Temperature = 20;
        Body = ParticleBody.Liquid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;
}

public class IronParticle : Particle
{
    private static readonly float Density = 7800f;
    private static readonly uint Color = 0xA19D94;

    public IronParticle(int x, int y) : base(x, y)
    {
        Temperature = 20;
        Body = ParticleBody.Solid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;
}

public class OxygenParticle : Particle
{
    private static readonly float Density = 1.4f;
    private static readonly uint Color = 0x99E2FA;

    public OxygenParticle(int x, int y) : base(x, y)
    {
        Temperature = 20;
        Body = ParticleBody.Solid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;
}
