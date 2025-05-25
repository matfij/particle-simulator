using SimulatorEngine;
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

    private void OnDownload(object sender, EventArgs e)
    {
        Console.WriteLine("TODO - download from S3");
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
