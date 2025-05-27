using System.Xml.Linq;
using SimulatorEngine;
using SimulatorEngine.Particles;
using SimulatorUI.Api;

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
                MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", ex.Message, "Close"));
            }
            catch
            {
                MainThread.BeginInvokeOnMainThread(async () => 
                    await DisplayAlert("Error", "Unable to download simulations, please try again later.", "Close"));
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
            SimulationList.ItemsSource = simulations;
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
                await DisplayAlert("Success", $"{name} was downloaded.", "Close");
                await Navigation.PopModalAsync();
            }
            catch (Exception ex) when (ex is FormatException || ex is HttpRequestException) 
            {
                await DisplayAlert("Error", ex.Message, "Close");
            }
            catch
            {
                await DisplayAlert("Error", "Unknown error, please try again later", "Close");
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
