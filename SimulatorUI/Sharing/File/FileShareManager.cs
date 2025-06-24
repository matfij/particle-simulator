using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
using SimulatorUI.Api;
using SimulatorUI.Resources.Locales;
using SimulatorEngine;

namespace SimulatorUI.Sharing.File;

public class FileShareManager(IParticlesManager particlesManager) : IShareManager
{
    private IParticlesManager _particlesManager = particlesManager;

    public Task<Stream> LoadSimulation(string simulationId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task ShareSimulation(
        string? simulationName = default,
        string? simulationData = default,
        CancellationToken token = default)
    {
        var name = String.Format(AppStrings.SimulationName, DateTime.Now.ToString("yyyy-MM-dd"));
        var data = SimulationSerializer.Serialize(_particlesManager.Particles);

        using var stream = new MemoryStream(Encoding.Default.GetBytes(data));
        var fileSaverResult = await FileSaver.Default.SaveAsync(name, stream, token);

        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make(String.Format(AppStrings.FileSaveSuccess, fileSaverResult.FilePath)).Show(token);
        }
        else
        {
            await Toast.Make(String.Format(AppStrings.FileSaveError, fileSaverResult.Exception.Message)).Show(token);
        }
    }

    public Task<IEnumerable<SimulationPreview>> LoadSimulationsPreview(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
