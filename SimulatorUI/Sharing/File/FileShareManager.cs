using SimulatorUI.Api;

namespace SimulatorUI.Sharing.File;

public class FileShareManager : IShareManager
{
    public Task<Stream> LoadSimulation(string simulationId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task ShareSimulation(string simulationName, string simulationData, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SimulationPreview>> LoadSimulationsPreview(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
