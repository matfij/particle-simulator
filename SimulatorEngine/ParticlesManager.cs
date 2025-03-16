using System.Diagnostics;
using System.Numerics;

namespace SimulatorEngine;

public class ParticlesManager
{
    public TimeSpan LoopTime { private set; get; } = new();
    private static readonly float _dt = 0.2f;
    private static readonly float _gravity = 0.05f;
    private readonly (int Width, int Height) _canvasSize = (1200, 600);
    private readonly System.Timers.Timer _simulationTimer = new(20);
    private readonly ParticlesPool _particlesPool = new();
    private readonly HashSet<Particle> _particles = [];
    private readonly LiquidManager _liquidManager;
    private readonly Stopwatch _stopwatch = new();
    private bool _particlesLock = false;

    public ParticlesManager()
    {
        _liquidManager = new(_dt, _gravity);
        _simulationTimer.Elapsed += (sender, args) => Tick();
        _simulationTimer.Start();
    }

    public IEnumerable<Particle> GetParticles => _particles;

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
                if (!_particles.Contains(_particlesPool.GetParticle(position, kind)) && dx * dx + dy * dy <= radiusSquare)
                {
                    _particles.Add(_particlesPool.GetParticle(position, kind));
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

        List<Particle> particlesToRemove = [];
        int radiusSquare = radius * radius;

        foreach (var particle in _particles)
        {
            var deltaFromCenter = Vector2.DistanceSquared(particle.Position, center);
            if (deltaFromCenter < radiusSquare)
            {
                particlesToRemove.Add(particle);
            }
        }

        foreach (var particle in particlesToRemove)
        {
            _particles.Remove(particle);
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

        List<(Particle, Vector2)> movedParticles = [];
        HashSet<Vector2> occupiedPositions = [];
        HashSet<Vector2> liquidPositions = [];

        foreach (var particle in _particles)
        {
            occupiedPositions.Add(particle.Position);
            if (particle.Body == ParticleBody.Liquid)
            {
                liquidPositions.Add(particle.Position);
            }
        }

        foreach (var particle in _particles)
        {
            Vector2 newPosition = particle.Position;

            switch (particle.Body)
            {
                case ParticleBody.Gas:
                    break;
                case ParticleBody.Liquid:
                    newPosition = _liquidManager.MoveLiquid(particle, occupiedPositions, liquidPositions);
                    break;
                case ParticleBody.Powder:
                    break;
                case ParticleBody.Solid:
                    break;
            }

            if (newPosition != particle.Position)
            {
                movedParticles.Add((particle, newPosition));
                occupiedPositions.Remove(particle.Position);
                occupiedPositions.Add(newPosition);
            }
        }

        foreach (var (particle, newPosition) in movedParticles)
        {
            _particles.Remove(particle);
            if (!IsOutOfBounds(newPosition))
            {
                _particles.Add(_particlesPool.GetParticle(newPosition, particle.GetKind()));
            }
        }

        _stopwatch.Stop();
        LoopTime = _stopwatch.Elapsed;
        _particlesLock = false;
    }

    private bool IsOutOfBounds(Vector2 position)
    {
        return position.X < 0 || position.X > _canvasSize.Width || position.Y < 0 || position.Y > _canvasSize.Height;
    }
}
