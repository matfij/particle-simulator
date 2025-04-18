namespace SimulatorEngine.Particles;

public enum PhaseTransitionDirection
{
    Up,
    Down,
}

public readonly struct PhaseTransition
{
    public PhaseTransitionDirection Direction { get; init; }
    public float Temperature { get; init; }
    public ParticleKind ResultKind { get; init; }
}
