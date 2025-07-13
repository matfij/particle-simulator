using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using SimulatorEngine;
using SimulatorEngine.Particles;
using SimulatorUI.Components;
using SimulatorUI.Definitions;
using SimulatorUI.Resources.Locales;
using SimulatorUI.Sharing;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SimulatorUI;


public partial class MainPage : ContentPage
{
    private readonly IParticlesManager _particlesManager;
    private bool _isPlaying = false;
    private readonly System.Timers.Timer _paintTimer = new(20);
    private readonly System.Timers.Timer _printTimer = new(200);
    private readonly SKBitmap _particlesBitmap = new(CanvasUtils._canvasSize.Width, CanvasUtils._canvasSize.Height);
    private (float X, float Y) _canvasScale = (1, 1);
    private (float X, float Y, float R) _cursor = (0, 0, 10);
    private ParticleKind _currentParticleKind = ParticleKind.Water;
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _paintTime = new();
    private readonly UploadPage _uploadPage;
    private readonly DownloadPage _downloadPage;
    private readonly DebugLevel _debugLevel;
    private readonly SharingMethod _sharingMethod;
    private readonly IShareManager _shareManager;
    private Particle? _markedParticle;

    public MainPage(
        IParticlesManager particlesManager,
        UploadPage uploadPage,
        DownloadPage downloadPage,
        IShareManager shareManager,
        IConfiguration configuration)
    {
        InitializeComponent();

        _particlesManager = particlesManager;
        _uploadPage = uploadPage;
        _downloadPage = downloadPage;
        _shareManager = shareManager;

        _debugLevel = Enum.TryParse(configuration["DebugLevel"], out DebugLevel level)
            ? level
            : DebugLevel.None;
        _sharingMethod = Enum.TryParse(configuration["SharingMethod"], out SharingMethod method)
            ? method
            : SharingMethod.None;

        _paintTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        _paintTimer.Start();

        ConfigureLayout();
    }

    private void ConfigureLayout()
    {
        switch (_debugLevel)
        {
            case DebugLevel.Full:
                _printTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(PrintParticleInfo);
                _printTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(PrintPerformanceInfo);
                _printTimer.Start();
                break;
            case DebugLevel.Basic:
                _printTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(PrintParticleInfo);
                _printTimer.Start();
                break;
        }

        switch (_sharingMethod)
        {
            case SharingMethod.Cloud:
                CloudShareButton.IsVisible = true;
                CloudDownloadButton.IsVisible = true;
                break;
            case SharingMethod.File:
                FileSaveButton.IsVisible = true;
                FileLoadButton.IsVisible = true;
                break;
        }
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        _stopwatch.Restart();
        try
        {
            _canvasScale = CanvasUtils.GetScale(args.Info.Width, args.Info.Height);
            UpdateBitmap();
            var canvas = args.Surface.Canvas;
            canvas.Clear(SKColors.Black);
            canvas.Scale(_canvasScale.X, _canvasScale.Y);

            canvas.DrawBitmap(_particlesBitmap, 0, 0);
            canvas.DrawCircle(_cursor.X, _cursor.Y, _cursor.R, CanvasUtils._cursorPaint);

            var (label, position, align) = CanvasUtils.GetMarkedParticleInfo(new(_cursor.X, _cursor.Y), _markedParticle);
            canvas.DrawText(label, position.X, position.Y, CanvasUtils._font, CanvasUtils._fontPaint);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Rendering error: {ex.Message}");
        }
        _stopwatch.Stop();
        _paintTime = _stopwatch.Elapsed;
    }

    private void PrintParticleInfo()
    {
        ParticleCountLabel.Text = $"{AppStrings.Particles}: {_particlesManager.ParticlesCount}";
    }

    private void PrintPerformanceInfo()
    {
        MoveTimeLabel.Text
            = $"{AppStrings.MoveTime}: {(int)_particlesManager.MoveTime.TotalMilliseconds} {AppStrings.MS}";
        InteractionTimeLabel.Text
            = $"{AppStrings.InteractionTime}: {(int)_particlesManager.InteractionTime.TotalMilliseconds} {AppStrings.MS}";
        HeatTransferTimeLabel.Text
            = $"{AppStrings.HeatTransferTime}: {(int)_particlesManager.HeatTransferTime.TotalMilliseconds} {AppStrings.MS}";
        PaintTimeLabel.Text
            = $"{AppStrings.PaintTime}: {(int)_paintTime.TotalMilliseconds} {AppStrings.MS}";
    }

    private unsafe void UpdateBitmap()
    {
        _particlesBitmap.Erase(SKColors.Black);
        var pixels = (uint*)_particlesBitmap.GetPixels();
        var maxIndex = CanvasUtils._canvasSize.Width * CanvasUtils._canvasSize.Height;

        foreach (var (position, particle) in _particlesManager.Particles)
        {
            int index = (int)position.X + (int)position.Y * CanvasUtils._canvasSize.Width;
            if (index >= 0 && index < maxIndex)
            {
                pixels[index] = particle.Color;
            }
        }
    }

    private void OnTouch(object sender, SKTouchEventArgs args)
    {
        if (args.ActionType == SKTouchAction.Moved)
        {
            _cursor.X = (int)(args.Location.X / _canvasScale.X);
            _cursor.Y = (int)(args.Location.Y / _canvasScale.Y);
            _markedParticle = _particlesManager.GetParticleAt(new(_cursor.X, _cursor.Y));
        }
        if (args.ActionType == SKTouchAction.WheelChanged)
        {
            var radius = (int)(_cursor.R + args.WheelDelta / 25);
            _cursor.R = Math.Clamp(radius, 1, 100);
        }
        if (args.ActionType == SKTouchAction.Pressed || args.ActionType == SKTouchAction.Moved)
        {
            if (args.MouseButton == SKMouseButton.Left)
            {
                _particlesManager.AddParticles(new(_cursor.X, _cursor.Y), (int)_cursor.R, _currentParticleKind);
            }
            if (args.MouseButton == SKMouseButton.Right)
            {
                _particlesManager.RemoveParticles(new(_cursor.X, _cursor.Y), (int)_cursor.R);
            }
        }
    }

    private void SetParticleKind(object sender, EventArgs e)
    {
        if (sender is Button selectedButton && selectedButton.CommandParameter is ParticleKind kind)
        {
            _currentParticleKind = kind;

            if (selectedButton.Parent is HorizontalStackLayout parentLayout)
            {
                foreach (var child in parentLayout.Children)
                {
                    if (child is Button button)
                    {
                        button.Opacity = 0.9;
                        button.BorderWidth = 1;
                        button.FontAttributes = FontAttributes.None;
                    }
                }
            }

            selectedButton.Opacity = 1;
            selectedButton.BorderWidth = 2;
            selectedButton.FontAttributes = FontAttributes.Bold;
        }
    }

    private void OnTogglePlaySimulation(object sender, EventArgs e)
    {
        TogglePlaySimulation(!_isPlaying);
    }

    private void TogglePlaySimulation(bool play)
    {
        _isPlaying = play;
        _particlesManager.TogglePlaySimulation(_isPlaying);
        if (_isPlaying)
        {
            PlayPauseButton.Source = "pause.png";
        }
        else
        {
            PlayPauseButton.Source = "play.png";
        }
    }

    private void OnClearSimulation(object sender, EventArgs e)
    {
        _particlesManager.ClearSimulation();
    }

    private async void OnOpenUploadPage(object sender, EventArgs e)
    {
        TogglePlaySimulation(false);
        await Navigation.PushModalAsync(_uploadPage);
    }

    private async void OnOpenDownloadPage(object sender, EventArgs e)
    {
        TogglePlaySimulation(false);
        await Navigation.PushModalAsync(_downloadPage);
    }

    private async void OnFileSave(object sender, EventArgs e)
    {
        TogglePlaySimulation(false);
        await _shareManager.ShareSimulation(String.Empty, String.Empty);
    }

    private async void OnFileLoad(object sender, EventArgs e)
    {
        TogglePlaySimulation(false);
        await _shareManager.LoadSimulation(String.Empty);
    }

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas?.InvalidateSurface());
    }
}
