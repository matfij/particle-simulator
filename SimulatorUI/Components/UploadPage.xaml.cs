using SimulatorEngine;
using SimulatorUI.Api;

namespace SimulatorUI;

public partial class UploadPage : ContentPage
{
    private readonly IApiManager _apiManager;
    private readonly IParticlesManager _particlesManager;

    public UploadPage(IApiManager apiManager, IParticlesManager particlesManager)
    {
        InitializeComponent();
        _apiManager = apiManager;
        _particlesManager = particlesManager;
    }

    private async void OnShare(object sender, EventArgs e)
    {
        var name = NameEntry.Text?.Trim() ?? string.Empty;
        if (!ValidateName(name))
        {
            return;
        }
        try
        {
            ToggleLoading(true);
            var simulationData = SimulationSerializer.Serialize(_particlesManager.Particles);
            await _apiManager.UploadSimulation(name, simulationData);
            ToggleLoading(false);
            await DisplayAlert("Success", $"{name} was shared.", "Close");
            await Navigation.PopModalAsync();
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Error", ex.Message, "Close");
        }
        catch
        {
            await DisplayAlert("Error", "Unknown error, please try again later", "Close");
        }
        finally
        {
            ToggleLoading(false);
        }
    }

    private bool ValidateName(string? name)
    {
        NameError.IsVisible = false;
        if (
            string.IsNullOrEmpty(name)
            || name.Length < 4
            || name.Length > 20
            || name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
        {
            NameError.IsVisible = true;
            return false;
        }
        return true;
    }

    private void ToggleLoading(bool loading)
    {
        ShareButton.IsVisible = !loading;
        CancelButton.IsVisible = !loading;
        LoadingIndicator.IsVisible = loading;
        EntryFrame.IsEnabled = !loading;
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
