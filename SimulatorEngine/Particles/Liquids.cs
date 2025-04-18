namespace SimulatorEngine.Particles;

public class WaterParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Water;
    private static readonly float Density = 1000f;
    private static readonly uint Color = 0x1CA3EC;

    public WaterParticle() : base()
    {
        Body = ParticleBody.Liquid;
        Transitions =
        [
            new()
            {
                Direction = PhaseTransitionDirection.Up,
                ResultKind = ParticleKind.Steam,
                Temperature = 100,
            }
        ];
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
        Transitions =
        [
            new()
            {
                Direction = PhaseTransitionDirection.Up,
                ResultKind = ParticleKind.Steam,
                Temperature = 102,
            }
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class AcidParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Acid;
    private static readonly float Density = 1100f;
    private static readonly uint Color = 0x89FF00;

    public AcidParticle() : base()
    {
        Body = ParticleBody.Liquid;
        Temperature = 300;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class LavaParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Lava;
    private static readonly float Density = 2200f;
    private static readonly uint Color = 0xCF1020;

    public LavaParticle() : base()
    {
        Body = ParticleBody.Liquid;
        Temperature = 1600;
        Transitions =
        [
            new()
            {
                Direction = PhaseTransitionDirection.Down,
                ResultKind = ParticleKind.Stone,
                Temperature = 1000,
            }
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
