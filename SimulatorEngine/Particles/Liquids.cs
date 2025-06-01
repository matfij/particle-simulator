namespace SimulatorEngine.Particles;

public class WaterParticle : Particle
{
    public override float Density => 1000f;

    public override uint Color => 0x1CA3EC;

    public override ParticleKind Kind => ParticleKind.Water;

    public override ParticleBody Body => ParticleBody.Liquid;

    private static readonly PhaseTransition[] _transitions = 
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Steam,
            Temperature = 100,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } = [];
}

public class SaltyWaterParticle : Particle
{
    public override float Density => 1025f;

    public override uint Color => 0x90AEBD;

    public override ParticleKind Kind => ParticleKind.SaltyWater;

    public override ParticleBody Body => ParticleBody.Liquid;

    private static readonly PhaseTransition[] _transitions =
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Steam,
            Temperature = 102,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } = [];
}

public class AcidParticle : Particle
{
    public override float Density => 1100f;

    public override uint Color => 0x89FF00;

    public override ParticleKind Kind => ParticleKind.Acid;

    public override ParticleBody Body => ParticleBody.Liquid;

    public override PhaseTransition[] Transitions => [];

    public override ParticleInteraction[] Interactions { get; set; } = [];

    public AcidParticle()
    {
        Temperature = 300;
    }
}

public class LavaParticle : Particle
{
    public override float Density => 2200f;

    public override uint Color => 0xCF1020;

    public override ParticleKind Kind => ParticleKind.Lava;

    public override ParticleBody Body => ParticleBody.Liquid;

    private static readonly PhaseTransition[] _transitions =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Down,
            ResultKind = ParticleKind.Stone,
            Temperature = 1000,
        }
    ];
    public override PhaseTransition[] Transitions => _transitions;

    public override ParticleInteraction[] Interactions { get; set; } = [];

    public LavaParticle()
    {
        Temperature = 1600;
    }
}
