using System.Diagnostics;
using System.Numerics;
using SimulatorEngine.Particles;

namespace SimulatorEngine;

public class ParticlesManager
{
    public TimeSpan LoopTime { private set; get; } = new();
    private static readonly float _dt = 0.2f;
    private static readonly float _gravity = 0.05f;
    private readonly (int Width, int Height) _canvasSize = (1200, 600);
    private readonly System.Timers.Timer _simulationTimer = new(20);
    private readonly ParticlesPool _particlesPool = new();
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
        _simulationTimer.Start();
    }

    public IDictionary<Vector2, Particle> GetParticles => _particles;

    public int GetParticlesCount => _particles.Count;

    public void AddParticles(Vector2 center, int radius, ParticleKind kind)
    {
        if (_particlesLock)
        {
            return;
        }
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
                    _particles.Add(position, _particlesPool.GetParticle(kind));
                }
            }
        }

        _particlesLock = false;
    }

    public void RemoveParticles(Vector2 center, int radius)
    {
        if (_particlesLock)
        {
            return;
        }
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

    private void Tick()
    {
        if (_particlesLock)
        {
            return;
        }
        _particlesLock = true;
        _stopwatch.Restart();

        var particles = new Dictionary<Vector2, Particle>(_particles);

        foreach (var (position, particle) in _particles.OrderBy(p => p.Value.GetDensity()))
        {
            var newPosition = position;

            switch (particle.Body)
            {
                case ParticleBody.Gas:
                    newPosition = _gasManager.MoveGas(position, particle, particles);
                    break;
                case ParticleBody.Liquid:
                    newPosition = _liquidManager.MoveLiquid(position, particle, particles);
                    break;
                case ParticleBody.Powder:
                    newPosition = _powderManager.MovePowder(position, particle, particles);
                    break;
            }

            if (newPosition != position)
            {
                particles.Remove(position);
                if (!IsOutOfBounds(newPosition) && !particles.ContainsKey(newPosition))
                {
                    particles.Add(newPosition, particle);
                }
            }
        }

        _particles = new Dictionary<Vector2, Particle>(particles);

        _stopwatch.Stop();
        LoopTime = _stopwatch.Elapsed;
        _particlesLock = false;
    }

    private bool IsOutOfBounds(Vector2 position)
    {
        return position.X < 0 || position.X > _canvasSize.Width || position.Y < 0 || position.Y > _canvasSize.Height;
    }
}
