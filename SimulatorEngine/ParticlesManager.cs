using System.Diagnostics;
using System.Numerics;
using SimulatorEngine.Managers;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public interface IParticlesManager
{
    IReadOnlyDictionary<Vector2, Particle> Particles { get; }
    int ParticlesCount { get; }
    TimeSpan MoveTime { get; }
    TimeSpan InteractionTime { get; }
    TimeSpan HeatTransferTime { get; }

    void AddParticles(Vector2 center, int radius, ParticleKind kind);
    void RemoveParticles(Vector2 center, int radius);
    void TogglePlaySimulation(bool play);
    void OverrideSimulation(IReadOnlyDictionary<Vector2, Particle> particles);
    void ClearSimulation();
}

public class ParticlesManager : IParticlesManager
{
    public TimeSpan MoveTime { private set; get; } = new();
    public TimeSpan InteractionTime { private set; get; } = new();
    public TimeSpan HeatTransferTime { private set; get; } = new();
    private static readonly float _dt = 0.2f;
    private static readonly float _gravity = 0.025f;
    private readonly (int Width, int Height) _canvasSize = (1200, 600);
    private readonly System.Timers.Timer _simulationTimer = new(20);
    private Dictionary<Vector2, Particle> _particles = [];
    private readonly LiquidManager _liquidManager;
    private readonly PowderManager _powderManager;
    private readonly GasManager _gasManager;
    private readonly Stopwatch _stopwatch = new();
    private bool _particlesLock = false;

    public ParticlesManager()
    {
        _liquidManager = new(_dt, _gravity);
        _powderManager = new(_dt, _gravity);
        _gasManager = new(_dt, _gravity);
        _simulationTimer.Elapsed += (sender, args) => Tick();
    }

    public IReadOnlyDictionary<Vector2, Particle> Particles => _particles;

    public int ParticlesCount => _particles.Count;

    public void AddParticles(Vector2 center, int radius, ParticleKind kind)
    {
        while (_particlesLock) { Thread.Yield(); }
        _particlesLock = true;

        int radiusSquare = radius * radius;
        for (int dx = -radius; dx <= radius; dx++)
        {
            if (center.X + dx > _canvasSize.Width || center.X + dx < 0)
            {
                continue;
            }
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (center.Y + dy > _canvasSize.Height || center.Y + dy < 0)
                {
                    continue;
                }
                Vector2 position = new(center.X + dx, center.Y + dy);
                if (!_particles.ContainsKey(position) && dx * dx + dy * dy <= radiusSquare)
                {
                    _particles.Add(position, ParticlesPool.GetParticle(kind));
                }
            }
        }

        _particlesLock = false;
    }

    public void RemoveParticles(Vector2 center, int radius)
    {
        while (_particlesLock) { Thread.Yield(); }
        _particlesLock = true;

        List<Vector2> positionsToRemove = [];
        int radiusSquare = radius * radius;

        foreach (var position in _particles.Keys)
        {
            var deltaFromCenter = Vector2.DistanceSquared(position, center);
            if (deltaFromCenter < radiusSquare)
            {
                positionsToRemove.Add(position);
            }
        }

        foreach (var position in positionsToRemove)
        {
            _particles.Remove(position);
        }

        _particlesLock = false;
    }

    public void TogglePlaySimulation(bool play)
    {
        if (play)
        {
            _simulationTimer.Start();
        }
        else
        {
            _simulationTimer.Stop();
        }
    }

    public void ClearSimulation()
    {
        while (_particlesLock) { Thread.Yield(); }
        _particlesLock = true;

        _particles = [];

        _particlesLock = false;
    }

    public void OverrideSimulation(IReadOnlyDictionary<Vector2, Particle> particles)
    {
        while (_particlesLock) { Thread.Yield(); }
        _particlesLock = true;

        _particles = new Dictionary<Vector2, Particle>(particles);

        _particlesLock = false;
    }

    private void Tick()
    {
        if (_particlesLock) { return; }
        _particlesLock = true;

        _stopwatch.Restart();
        var particlesToMove = new Dictionary<Vector2, Particle>(_particles);
        foreach (var (position, particle) in _particles.OrderBy(p => p.Value.Density))
        {
            var newPosition = position;
            switch (particle.Body)
            {
                case ParticleBody.Gas:
                case ParticleBody.Plasma:
                    newPosition = _gasManager.MoveGas(position, particle, particlesToMove);
                    break;
                case ParticleBody.Liquid:
                    newPosition = _liquidManager.MoveLiquid(position, particle, particlesToMove);
                    break;
                case ParticleBody.Powder:
                    newPosition = _powderManager.MovePowder(position, particle, particlesToMove);
                    break;
            }
            if (newPosition != position)
            {
                particlesToMove.Remove(position);
                if (!IsOutOfBounds(newPosition) && !particlesToMove.ContainsKey(newPosition))
                {
                    particlesToMove.Add(newPosition, particle);
                }
            }
        }
        _stopwatch.Stop();
        MoveTime = _stopwatch.Elapsed;

        _stopwatch.Restart();
        var particlesToInteract = new Dictionary<Vector2, Particle>();
        foreach (var (position, particle) in particlesToMove)
        {
            var newParticle = InteractionManager.DoInteractions(position, particle, particlesToMove);
            if (newParticle != null)
            {
                particlesToInteract.Add(position, newParticle);
            }
        }
        _stopwatch.Stop();
        InteractionTime = _stopwatch.Elapsed;

        _stopwatch.Restart();
        TemperatureManager.TransferHeat(particlesToInteract);
        _stopwatch.Stop();
        HeatTransferTime = _stopwatch.Elapsed;

        _particles = new Dictionary<Vector2, Particle>(particlesToInteract);

        _particlesLock = false;
    }

    private bool IsOutOfBounds(Vector2 position)
    {
        return position.X < 0 || position.X > _canvasSize.Width || position.Y < 0 || position.Y > _canvasSize.Height;
    }
}
