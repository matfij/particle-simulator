namespace SimulatorEngine.Particles;

public class IronParticle : Particle
{
    public override float Density => 7800f;

    public override uint Color => ComputeTemperatureAdjustedColor(0xA1, 0x9D, 0x94, 6f);

    public override ParticleKind Kind => ParticleKind.Iron;

    public override ParticleBody Body => ParticleBody.Solid;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1500,
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

public class PlantParticle : Particle
{
    public override float Density => 400f;

    public override uint Color => 0x4CD038;

    public override ParticleKind Kind => ParticleKind.Plant;

    public override ParticleBody Body => ParticleBody.Solid;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Fire,
            Temperature = 250,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } =
    [
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.SaltyWater,
            Ticks = 15,
        },
        new()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Acid,
            Ticks = 3,
        },
        new()
        {
            Result = InteractionResult.Merge,
            NeighborKind = ParticleKind.Fire,
            ResultKind = ParticleKind.Fire,
            Ticks = 3,
        },
    ];
}
