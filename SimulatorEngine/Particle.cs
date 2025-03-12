using System.Numerics;

namespace SimulatorEngine;

public enum ParticleKind
{
    None,
    Sand,
    Water,
    Iron,
    Oxygen,
}

public enum ParticleBody
{
    Solid,
    Powder,
    Liquid,
    Gas,
}

public abstract class Particle(Vector2 position)
{
    public readonly Vector2 Position = position;
    public int Temperature { get; set; }
    public ParticleBody Body { get; set; }

    public abstract ParticleKind GetKind();

    public abstract uint GetColor();

    public abstract float GetDensity();

    public override bool Equals(object? obj) => obj is Particle p && p.Position == Position;

    public override int GetHashCode() => HashCode.Combine(Position);
}

public class SandParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Sand;
    private static readonly float Density = 1442f;
    private static readonly uint Color = 0xF6D7B0;

    public SandParticle(Vector2 position) : base(position)
    {
        Temperature = 20;
        Body = ParticleBody.Powder;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;
    
    public override ParticleKind GetKind() => Kind;
}

public class WaterParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Water;
    private static readonly float Density = 1000f;
    private static readonly uint Color = 0x1CA3EC;

    public WaterParticle(Vector2 position) : base(position)
    {
        Temperature = 20;
        Body = ParticleBody.Liquid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class IronParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Iron;
    private static readonly float Density = 7800f;
    private static readonly uint Color = 0xA19D94;

    public IronParticle(Vector2 position) : base(position)
    {
        Temperature = 20;
        Body = ParticleBody.Solid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class OxygenParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Oxygen;
    private static readonly float Density = 1.4f;
    private static readonly uint Color = 0x99E2FA;

    public OxygenParticle(Vector2 position) : base(position)
    {
        Temperature = 20;
        Body = ParticleBody.Gas;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
