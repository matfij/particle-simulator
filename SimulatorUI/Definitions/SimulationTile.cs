using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimulatorUI.Resources.Locales;

namespace SimulatorUI.Definitions;

public class SimulationTile : INotifyPropertyChanged
{
    public required string Id { get; init; }
    public required string Name { get; init; }

    private int _downloads;
    public required int Downloads
    {
        get => _downloads;
        set
        {
            _downloads = value;
            OnPropertyChanged();
            DownloadsLabel = string.Format(AppStrings.Downloads, _downloads);
        }
    }

    private string _downloadsLabel = string.Empty;
    public string DownloadsLabel
    {
        get => _downloadsLabel;
        private set
        {
            _downloadsLabel = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}