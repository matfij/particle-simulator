using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public static class ParticlesDataManager
{
    private readonly static Dictionary<ParticleKind, ParticleData> _particleData = new()
    {
        { ParticleKind.Sand, new SandData() },
        { ParticleKind.Water, new WaterData() },
        { ParticleKind.Iron, new IronData() },
        { ParticleKind.Iron, new OxygenData() },
        { ParticleKind.Iron, new SaltData() },
        { ParticleKind.Iron, new SaltyWaterData() },
        { ParticleKind.Iron, new AcidData() },
        { ParticleKind.Iron, new LavaData() },
        { ParticleKind.Iron, new StoneData() },
        { ParticleKind.Iron, new SteamData() },
    };

    public static ParticleData GetParticleData(ParticleKind kind) => _particleData[kind];
}
