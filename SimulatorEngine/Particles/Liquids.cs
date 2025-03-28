namespace SimulatorEngine.Particles;

public class WaterParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Water;
    private static readonly float Density = 1000f;
    private static readonly uint Color = 0x1CA3EC;

    public WaterParticle() : base()
    {
        Body = ParticleBody.Liquid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class SaltyWaterParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.SaltyWater;
    private static readonly float Density = 1025f;
    private static readonly uint Color = 0x90AEBD;

    public SaltyWaterParticle() : base()
    {
        Body = ParticleBody.Liquid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
