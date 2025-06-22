using SimulatorUI.Api;

namespace SimulatorUI.Sharing;

public interface IShareManager
{
    Task ShareSimulation(string simulationName, string simulationData, CancellationToken token = default);
    Task<Stream> LoadSimulation(string simulationId, CancellationToken token = default);
    Task<IEnumerable<SimulationPreview>> LoadSimulationsPreview(CancellationToken token = default);
}
