namespace SimulatorEngine.Particles;

public class SandParticle : Particle
{
    public override float Density => 1600f;

    public override uint Color => ComputeTemperatureAdjustedColor(0xF6, 0xD7, 0xB0, 8f);

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

    public override uint Color => ComputeTemperatureAdjustedColor(0xFC, 0xF9, 0xF3, 8f);

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

    public override uint Color => ComputeTemperatureAdjustedColor(0x78, 0x7A, 0x79, 6f);

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
