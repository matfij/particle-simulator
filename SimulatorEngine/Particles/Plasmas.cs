namespace SimulatorEngine.Particles;

public class FireParticle : Particle
{
    public override float Density => 0.45f;

    public override uint Color => 0xFF4412;

    public override ParticleKind Kind => ParticleKind.Fire;

    public override ParticleBody Body => ParticleBody.Plasma;

    public override PhaseTransition[] Transitions => [];

    public override ParticleInteraction[] Interactions { get; set; } =
    [
        new ()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Water,
            Ticks = 2,
        },
        new ()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.SaltyWater,
            Ticks = 2,
        },
        new ()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Steam,
            Ticks = 4,
        },
        new ()
        {
            Result = InteractionResult.RemoveSelf,
            NeighborKind = ParticleKind.Smoke,
            Ticks = 5,
        },
        new ()
        {
            Result = InteractionResult.ExipreTransform,
            ResultKind = ParticleKind.Smoke,
            Ticks = 40,
        },
    ];

    public FireParticle()
    {
        Temperature = 500;
    }
}

public class SmokeParticle : Particle
{
    public override float Density => 0.3f;

    public override uint Color => 0x848884;

    public override ParticleKind Kind => ParticleKind.Smoke;

    public override ParticleBody Body => ParticleBody.Plasma;

    public override PhaseTransition[] Transitions => [];

    public override ParticleInteraction[] Interactions { get; set; } = [];

    public SmokeParticle()
    {
        Temperature = 150;
    }
}
