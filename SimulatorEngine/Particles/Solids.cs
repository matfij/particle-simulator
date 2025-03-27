namespace SimulatorEngine.Particles;

public class IronParticle : Particle
{
    private static readonly ParticleKind Kind = ParticleKind.Iron;
    private static readonly float Density = 7800f;
    private static readonly uint Color = 0xA19D94;

    public IronParticle()
    {
        Temperature = 20;
        Body = ParticleBody.Solid;
    }

    public override uint GetColor() => Color;

    public override float GetDensity() => Density;

    public override ParticleKind GetKind() => Kind;
}
