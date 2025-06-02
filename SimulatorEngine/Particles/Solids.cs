namespace SimulatorEngine.Particles;

public class IronParticle : Particle
{
    public override float Density => 7800f;

    public override uint Color => 0xA19D94;

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
