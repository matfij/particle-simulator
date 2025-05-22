namespace SimulatorEngine.Particles;

public class OxygenData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Gas;
    public override float Density { get; init; } = 1.4f;
    public override uint Color { get; init; } = 0x99E2FA;
}

public class OxygenParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Oxygen;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [];
}

public class SteamData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Gas;
    public override float Density { get; init; } = 15f;
    public override uint Color { get; init; } = 0xC7D5E0;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Down,
            ResultKind = ParticleKind.Water,
            Temperature = 99,
        }
    ];
}

public class SteamParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Steam;
    public float Temperature { get; set; } = 128;
    public List<int> InteractionTicks { get; init; } = [];
}
