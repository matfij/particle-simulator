namespace SimulatorEngine.Particles;

public class SandParticle : Particle
{
    public override float Density => 1600f;

    public override uint Color => 0xF6D7B0;

    public override ParticleKind Kind => ParticleKind.Sand;

    public override ParticleBody Body => ParticleBody.Powder;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1700,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
            Ticks = 8,
        },
    ];
}

public class SaltParticle : Particle
{
    public override float Density => 2100f;

    public override uint Color => 0xFCF9F3;

    public override ParticleKind Kind => ParticleKind.Salt;

    public override ParticleBody Body => ParticleBody.Powder;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1700,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } =
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

public class StoneParticle : Particle
{
    public override float Density => 2500f;

    public override uint Color => 0x787A79;

    public override ParticleKind Kind => ParticleKind.Stone;

    public override ParticleBody Body => ParticleBody.Powder;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1200,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
            Ticks = 12,
        },
    ];
}
