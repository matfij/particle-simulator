using SimulatorEngine.Particles;

namespace SimulatorEngine.Managers;

public static class ParticlesDataManager
{
    private readonly static Dictionary<ParticleKind, ParticleData> _particleData = new()
    {
        { ParticleKind.Sand, new SandData() },
        { ParticleKind.Water, new WaterData() },
        { ParticleKind.Iron, new IronData() },
        { ParticleKind.Oxygen, new OxygenData() },
        { ParticleKind.Salt, new SaltData() },
        { ParticleKind.SaltyWater, new SaltyWaterData() },
        { ParticleKind.Acid, new AcidData() },
        { ParticleKind.Lava, new LavaData() },
        { ParticleKind.Stone, new StoneData() },
        { ParticleKind.Steam, new SteamData() },
    };

    public static ParticleData GetParticleData(ParticleKind kind) => _particleData[kind];
}
