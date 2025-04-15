namespace SimulatorEngine.Particles;

public class OxygenParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Oxygen;
    private static readonly float Density = 1.4f;
    private static readonly uint Color = 0x99E2FA;

    public OxygenParticle() : base()
    {
        Body = ParticleBody.Gas;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}

public class SteamParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Steam;
    private static readonly float Density = 15f;
    private static readonly uint Color = 0xC7D5E0;

    public SteamParticle() : base()
    {
        Body = ParticleBody.Gas;
        Temperature = 128;
        Transitions =
        [
            new()
            {
                Direction = PhaseTransitionDirection.Down,
                ResultKind = ParticleKind.Water,
                Temperature = 99,
            }
        ];
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
