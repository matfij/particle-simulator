namespace SimulatorEngine.Particles;

public class SandData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 1600f;
    public override uint Color { get; init; } = 0xF6D7B0;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1700,
        }
    ];
    public override List<ParticleInteraction> Interactions { get; init; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
        },
    ];
}

public class SandParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Sand;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [8];
}

public class SaltData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 2100f;
    public override uint Color { get; init; } = 0xFCF9F3;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 800,
        }
    ];
    public override List<ParticleInteraction> Interactions { get; init; } =
    [
        new()
        {
            Result = InteractionResult.Merge,
            NeighborKind = ParticleKind.Water,
            ResultKind = ParticleKind.SaltyWater,
        },
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
        },
    ];
}

public class SaltParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Salt;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [12, 6];
}

public class StoneData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Powder;
    public override float Density { get; init; } = 2500f;
    public override uint Color { get; init; } = 0x787A79;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1200,
        }
    ];
    public override List<ParticleInteraction> Interactions { get; init; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
        },
    ];
}

public class StoneParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Stone;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [12];
}
