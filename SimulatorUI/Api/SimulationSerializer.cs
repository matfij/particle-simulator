using SimulatorEngine;
using SimulatorEngine.Particles;
using System.Numerics;
using System.Text;

namespace SimulatorUI.Api;

public static class SimulationSerializer
{
    private readonly static string _versionHeader = "particle-simulator v 1.0.0";
    private readonly static string _attributeSeparator = ":";

    public static string Serialize(IReadOnlyDictionary<Vector2, Particle> particles)
    {
        var simulationData = new StringBuilder();
        simulationData.AppendLine(_versionHeader);

        foreach (var (position, particle) in particles)
        {
            simulationData.AppendLine(
                $"{position.X}{_attributeSeparator}{position.Y}{_attributeSeparator}" +
                $"{particle.Kind}{_attributeSeparator}{particle.Temperature}");
        }

        return simulationData.ToString();
    }

    public static async Task<IReadOnlyDictionary<Vector2, Particle>> Deserialize(Stream simulationData)
    {
        try
        {
            var simulation = new Dictionary<Vector2, Particle>();
            var headerLine = true;

            using var reader = new StreamReader(simulationData);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (headerLine)
                {
                    headerLine = false;
                    continue; // Add migration when more version are available
                }

                string[] parts = line!.Split(_attributeSeparator);
                var x = float.Parse(parts[0]);
                var y = float.Parse(parts[1]);
                var kind = (ParticleKind)Enum.Parse(typeof(ParticleKind), parts[2]);
                var temperature = float.Parse(parts[3]);

                var position = new Vector2(x, y);
                var particle = ParticlesPool.GetParticle(kind);
                particle.Temperature = temperature;

                simulation.Add(position, particle);
            }

            return simulation;
        }
        catch (Exception ex)
        {
            throw new FormatException($"Invalid simulation data format (expected {_versionHeader})", ex);
        }
    }
}
