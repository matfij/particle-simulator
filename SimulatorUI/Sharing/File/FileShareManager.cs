using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
using SimulatorUI.Api;
using SimulatorUI.Resources.Locales;
using SimulatorEngine;

namespace SimulatorUI.Sharing.File;

public class FileShareManager(IParticlesManager particlesManager) : IShareManager
{
    private readonly string _format = ".sim";
    private readonly FilePickerFileType _fileType = new
    (
        new Dictionary<DevicePlatform, IEnumerable<string>> 
        {
            { DevicePlatform.WinUI, new[] { ".sim" } } 
        }
    );
    private readonly IParticlesManager _particlesManager = particlesManager;

    public async Task<Stream> LoadSimulation(string? simulationId, CancellationToken token = default)
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions() { FileTypes = _fileType });
            if (file is not null)
            {
                using var data = await file.OpenReadAsync();
                var simulation = await SimulationSerializer.Deserialize(data);
                _particlesManager.OverrideSimulation(simulation);
            }
        }
        catch
        {
            await Toast.Make(AppStrings.LoadSimulationError).Show(token);
        }
        return new MemoryStream();
    }

    public async Task ShareSimulation(string? simulationName, string? simulationData, CancellationToken token = default)
    {
        var name = String.Format(AppStrings.SimulationName, DateTime.Now.ToString("yyyy-MM-dd")) + _format;
        var data = SimulationSerializer.Serialize(_particlesManager.Particles);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var fileSaverResult = await FileSaver.Default.SaveAsync(name, stream, token);

        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make(String.Format(AppStrings.FileSaveSuccess, fileSaverResult.FilePath))
                .Show(token);
        }
        else
        {
            await Toast.Make(String.Format(AppStrings.FileSaveError, fileSaverResult.Exception.Message ?? AppStrings.UnknownError))
                .Show(token);
        }
    }

    public Task<IEnumerable<SimulationPreview>> LoadSimulationsPreview(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
