namespace SimulatorEngine.Particles;

public class IronData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Solid;
    public override float Density { get; init; } = 7800f;
    public override uint Color { get; init; } = 0xA19D94;
    public new List<PhaseTransition> Transitions { get; init; } =
    [
        new()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Lava,
            Temperature = 1500,
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

public class IronParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Iron;
    public float Temperature { get; set; } = 20;
}
