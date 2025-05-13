namespace SimulatorEngine.Particles;

public class SandData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 1600f;
    public override uint Color { get; init; } = 0xF6D7B0;
    public new List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1700,
        }
    ];
    public new List<ParticleInteraction> Interactions { get; init; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
            Ticks = 8,
        },
    ];
}

public class SandParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Sand;
    public float Temperature { get; set; } = 20;
}

public class SaltData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 2100f;
    public override uint Color { get; init; } = 0xFCF9F3;
    public new List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 800,
        }
    ];
    public new List<ParticleInteraction> Interactions { get; init; } =
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

public class SaltParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Salt;
    public float Temperature { get; set; } = 20;
}

public class StoneData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 2500f;
    public override uint Color { get; init; } = 0x787A79;
    public new List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1200,
        }
    ];
    public new List<ParticleInteraction> Interactions { get; init; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
            Ticks = 12,
        },
    ];
}

public class StoneParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Stone;
    public float Temperature { get; set; } = 20;
}
