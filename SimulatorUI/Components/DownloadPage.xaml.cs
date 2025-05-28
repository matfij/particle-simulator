using SimulatorEngine;
using SimulatorUI.Api;
using SimulatorUI.Resources.Locales;

namespace SimulatorUI.Components;

public partial class DownloadPage : ContentPage
{
    private bool _loaded = false;
    private readonly IApiManager _apiManager;
    private readonly IParticlesManager _particlesManager;

    public DownloadPage(IApiManager apiManager, IParticlesManager particlesManager)
    {
        InitializeComponent();
        _apiManager = apiManager;
        _particlesManager = particlesManager;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(async () =>
        {
            try
            {
                if (!_loaded)
                {
                    MainThread.BeginInvokeOnMainThread(() => LoadingIndicator.IsVisible = true);
                    await FetchSimulations();
                }
            }
            catch (HttpRequestException ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => 
                    await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.Close));
            }
            catch
            {
                MainThread.BeginInvokeOnMainThread(async () => 
                    await DisplayAlert(AppStrings.Error, AppStrings.DownloadGenericError, AppStrings.Close));
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() => LoadingIndicator.IsVisible = false);
            }
        });
    }

    private async Task FetchSimulations()
    {
        var simulations = await _apiManager.DownloadSimulationsPreview();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            SimulationList.ItemsSource = simulations.Select(s => 
                new { s.Id, s.Name, Downloads = string.Format(AppStrings.Downloads, s.Downloads)});
            _loaded = true;
        });
    }

    private async void OnDownload(object sender, EventArgs e)
    {
        if (sender is ImageButton downloadButton && downloadButton.CommandParameter is string id)
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                SimulationList.IsVisible = false;

                var data = await _apiManager.DownloadSimulation(id);
                var simulation = await SimulationSerializer.Deserialize(data);

                _particlesManager.OverrideSimulation(simulation);

                var name = SimulationList.ItemsSource.Cast<SimulationPreview>().First(preview => preview.Id == id).Name;
                await DisplayAlert(AppStrings.Success, string.Format(AppStrings.SimulationDownloaded, name), AppStrings.Close);
                await Navigation.PopModalAsync();
            }
            catch (Exception ex) when (ex is FormatException || ex is HttpRequestException) 
            {
                await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.Close);
            }
            catch
            {
                await DisplayAlert(AppStrings.Error, AppStrings.UnknownError, AppStrings.Close);
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                SimulationList.IsVisible = true;
            }
        }
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
