using SimulatorEngine;
using SimulatorUI.Api;
using SimulatorUI.Resources.Locales;

namespace SimulatorUI;

public partial class UploadPage : ContentPage
{
    private readonly IApiManager _apiManager;
    private readonly IParticlesManager _particlesManager;
    private readonly (int minLength, int maxLength) _nameConfig = (minLength: 4, maxLength: 12);
    private CancellationTokenSource? _cancellationTokenSource;

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
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            ToggleLoading(true);

            var simulationData = SimulationSerializer.Serialize(_particlesManager.Particles);
            await _apiManager.UploadSimulation(name, simulationData, _cancellationTokenSource.Token);

            ToggleLoading(false);

            await DisplayAlert(AppStrings.Success, string.Format(AppStrings.SimulationShared, name), AppStrings.Close);
            await Navigation.PopModalAsync();
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.Close);
        }
        catch
        {
            await DisplayAlert(AppStrings.Error, AppStrings.UnknownError, AppStrings.Close);
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
            || name.Length < _nameConfig.minLength
            || name.Length > _nameConfig.maxLength
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
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
        }

        await Navigation.PopModalAsync();
    }
}
