using SimulatorEngine;
using SimulatorUI.Api;

namespace SimulatorUI.Components;

public partial class DownloadPage : ContentPage
{
    public List<Simulation> Simulations { get; private set; } = [];
    private readonly IApiManager _apiManager;
    private readonly IParticlesManager _particlesManager;

    public DownloadPage(IApiManager apiManager, IParticlesManager particlesManager)
    {
        InitializeComponent();
        _apiManager = apiManager;
        _particlesManager = particlesManager;
        FetchSimulations();
    }

    private async void FetchSimulations()
    {
        LoadingIndicator.IsVisible = true;
      
        await Task.Delay(6000);
        Simulations =
        [
            new Simulation
            {
                Id = "test-1",
                Name = "Test",
                FileName = "test.json",
                Downloads = 100
            },
            new Simulation
            {
                Id = "test-2",
                Name = "Dummy",
                FileName = "dummy.json",
                Downloads = 20234
            },
        ];

        SimulationList.ItemsSource = Simulations;
        LoadingIndicator.IsVisible = false;
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