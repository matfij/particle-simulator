using System.Collections.ObjectModel;
using SimulatorEngine;
using SimulatorUI.Api;
using SimulatorUI.Definitions;
using SimulatorUI.Resources.Locales;

namespace SimulatorUI.Components;

public partial class DownloadPage : ContentPage
{
    private bool _loaded = false;
    private readonly IApiManager _apiManager;
    private readonly IParticlesManager _particlesManager;
    private ObservableCollection<SimulationTile> _simulationTiles = [];

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
            _simulationTiles = new ObservableCollection<SimulationTile>(simulations.Select(s => 
                new SimulationTile 
                { 
                    Id = s.Id, 
                    Name = s.Name, 
                    Downloads = s.Downloads,
                }));
            SimulationList.ItemsSource = _simulationTiles;
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

                var simulationTile = _simulationTiles.First(preview => preview.Id == id);
                simulationTile.Downloads += 1;

                await DisplayAlert(
                    AppStrings.Success, string.Format(AppStrings.SimulationDownloaded, simulationTile.Name), AppStrings.Close);
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
