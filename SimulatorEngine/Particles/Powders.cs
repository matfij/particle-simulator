namespace SimulatorEngine.Particles;

public class SandParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Sand;
    private static readonly float Density = 1600f;
    private static readonly uint Color = 0xF6D7B0;

    public SandParticle() : base()
    {
        Body = ParticleBody.Powder;
        Interactions =
        [
            new()
            {
                 Result = InteractionResult.RemoveSelf,
                 NeighborKind = ParticleKind.Acid,
                 Ticks = 8,
            },
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class SaltParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Salt;
    private static readonly float Density = 2100f;
    private static readonly uint Color = 0xFCF9F3;

    public SaltParticle() : base()
    {
        Body = ParticleBody.Powder;
        Interactions =
        [
             new()
             {
                 Result = InteractionResult.Merge,
                 NeighborKind = ParticleKind.Water,
                 Ticks = 12,
                 ResultKind = ParticleKind.SaltyWater,
             },
             new()
             {
                 Result = InteractionResult.RemoveSelf,
                 NeighborKind = ParticleKind.Acid,
                 Ticks = 6,
             },
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class StoneParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Stone;
    private static readonly float Density = 2500f;
    private static readonly uint Color = 0x787A79;

    public StoneParticle() : base()
    {
        Body = ParticleBody.Powder;
        Interactions =
        [
             new()
             {
                 Result = InteractionResult.RemoveSelf,
                 NeighborKind = ParticleKind.Acid,
                 Ticks = 12,
             },
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
