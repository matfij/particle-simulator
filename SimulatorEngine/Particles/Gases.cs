namespace SimulatorEngine.Particles;

public class OxygenParticle : Particle
{
    public override float Density => 1.4f;

    public override uint Color => 0x99E2FA;

    public override ParticleKind Kind => ParticleKind.Oxygen;

    public override ParticleBody Body => ParticleBody.Gas;

    public override PhaseTransition[] Transitions => [];

    public override ParticleInteraction[] Interactions { get; set; } =
    [
        new()
        {
            Result = InteractionResult.Merge,
            NeighborKind = ParticleKind.Fire,
            ResultKind = ParticleKind.Fire,
            Ticks = 2,
        },
    ];
}

public class SteamParticle : Particle
{
    public override float Density => 15f;

    public override uint Color => 0xC7D5E0;

    public override ParticleKind Kind => ParticleKind.Steam;

    public override ParticleBody Body => ParticleBody.Gas;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Down,
            ResultKind = ParticleKind.Water,
            Temperature = 99,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } = [];

    public SteamParticle()
    {
        Temperature = 128;
    }
}
