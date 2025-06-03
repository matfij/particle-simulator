namespace SimulatorEngine.Particles;

public class IronParticle : Particle
{
    public override float Density => 7800f;

    public override uint Color // 0xA19D94
    {
        get
        {
            int baseRed = 0xA1;
            int redShift = (int)(Temperature / 6);

            int red = baseRed + redShift;
            if (red > 255) red = 255;

            return (uint)((red << 16) | (0x9D << 8) | 0x94);
        }
    }

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
