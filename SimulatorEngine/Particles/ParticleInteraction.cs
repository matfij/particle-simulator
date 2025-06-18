namespace SimulatorEngine.Particles;

public enum InteractionResult
{
    Merge,
    RemoveNeighbor,
    RemoveSelf,
    RemoveBoth,
    ExipreTransform,
}

public class ParticleInteraction
{
    public InteractionResult Result { get; init; }
    public ParticleKind? NeighborKind { get; init; }
    public ParticleKind? ResultKind { get; init; }
    public int Ticks { get; set; }
}
