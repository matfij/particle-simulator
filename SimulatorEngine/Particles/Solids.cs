namespace SimulatorEngine.Particles;

public class IronParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Iron;
    private static readonly float Density = 7800f;
    private static readonly uint Color = 0xA19D94;
    private readonly int MaxTicksToDissolveInAcid = 5;
    private int _ticksToDissolveInAcid;
    public int TicksToDissolveInAcid
    {
        get => _ticksToDissolveInAcid;
        set => _ticksToDissolveInAcid = Math.Min(value, MaxTicksToDissolveInAcid);
    }

    public IronParticle() : base()
    {
        Body = ParticleBody.Solid;
        _ticksToDissolveInAcid = MaxTicksToDissolveInAcid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
