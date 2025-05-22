namespace SimulatorEngine.Particles;

public enum InteractionResult
{
    Merge,
    RemoveNeighbor,
    RemoveSelf,
    RemoveBoth,
}

public class ParticleInteraction
{
    public InteractionResult Result { get; init; }
    public ParticleKind NeighborKind { get; init; }
    public ParticleKind? ResultKind { get; init; }
}
