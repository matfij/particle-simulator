namespace SimulatorEngine.Particles;

public class WaterData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Liquid;
    public override float Density { get; init; } = 1000f;
    public override uint Color { get; init; } = 0x1CA3EC;

    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Steam,
            Temperature = 100,
        }
    ];
}

public class WaterParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Water;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [];
}

public class SaltyWaterData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Liquid;
    public override float Density { get; init; } = 1025f;
    public override uint Color { get; init; } = 0x90AEBD;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Up,
            ResultKind = ParticleKind.Steam,
            Temperature = 102,
        }
    ];
}

public class SaltyWaterParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.SaltyWater;
    public float Temperature { get; set; } = 20;
    public List<int> InteractionTicks { get; init; } = [];
}

public class AcidData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Liquid;
    public override float Density { get; init; } = 1100f;
    public override uint Color { get; init; } = 0x89FF00;
}

public class AcidParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Acid;
    public float Temperature { get; set; } = 300;
    public List<int> InteractionTicks { get; init; } = [];
}

public class LavaData : ParticleData
{
    public override ParticleBody Body { get; init; } = ParticleBody.Liquid;
    public override float Density { get; init; } = 2200f;
    public override uint Color { get; init; } = 0xCF1020;
    public override List<PhaseTransition> Transitions { get; init; } =
    [
        new ()
        {
            Direction = PhaseTransitionDirection.Down,
            ResultKind = ParticleKind.Stone,
            Temperature = 1000,
        }
    ];
}

public class LavaParticle : IParticle
{
    public ParticleKind Kind { get; init; } = ParticleKind.Lava;
    public float Temperature { get; set; } = 1600;
    public List<int> InteractionTicks { get; init; } = [];
}
