using System.Diagnostics;
using SimulatorEngine;
using SimulatorEngine.Particles;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SimulatorUI;

public partial class MainPage : ContentPage
{
    private static readonly (int Width, int Height) _canvasSize = (1200, 600);
    private static readonly SKPaint _cursorPaint = new()
    {
        StrokeWidth = 2,
        IsAntialias = true,
        Color = SKColors.GhostWhite,
        Style = SKPaintStyle.Stroke,
    };
    private readonly IParticlesManager _particlesManager;
    private readonly System.Timers.Timer _paintTimer = new(20);
    private readonly System.Timers.Timer _printTimer = new(200);
    private readonly SKBitmap _particlesBitmap = new(_canvasSize.Width, _canvasSize.Height);
    private (float X, float Y) _canvasScale = (1, 1);
    private (float X, float Y, float R) _cursor = (0, 0, 10);
    private ParticleKind _currentParticleKind = ParticleKind.Water;
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _paintTime = new();
    private readonly UploadPage _uploadPage;

    public MainPage(IParticlesManager particlesManager, UploadPage shareModal)
    {
        InitializeComponent();
        _particlesManager = particlesManager;
        _uploadPage = shareModal;
        _paintTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        _paintTimer.Start();
        _printTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(PrintPerformanceInfo);
        _printTimer.Start();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        _stopwatch.Restart();
        try
        {
            _canvasScale = (args.Info.Width / (float)_canvasSize.Width, args.Info.Height / (float)_canvasSize.Height);
            UpdateBitmap();
            var canvas = args.Surface.Canvas;
            canvas.Clear(SKColors.Black);
            canvas.Scale(_canvasScale.X, _canvasScale.Y);

            canvas.DrawBitmap(_particlesBitmap, 0, 0);
            canvas.DrawCircle(_cursor.X, _cursor.Y, _cursor.R, _cursorPaint);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Rendering error: {ex.Message}");
        }
        _stopwatch.Stop();
        _paintTime = _stopwatch.Elapsed;
    }

    private void PrintPerformanceInfo()
    {
        ParticleCountLabel.Text = $"Particles: {_particlesManager.ParticlesCount}";
        ComputeTimeLabel.Text = $"Compute time: {(int)_particlesManager.LoopTime.TotalMilliseconds} [ms]";
        PaintTimeLabel.Text = $"Paint time: {(int)_paintTime.TotalMilliseconds} [ms]";
    }

    private unsafe void UpdateBitmap()
    {
        _particlesBitmap.Erase(SKColors.Black);
        var pixels = (uint*)_particlesBitmap.GetPixels();
        var maxIndex = _canvasSize.Width * _canvasSize.Height;

        foreach (var (position, particle) in _particlesManager.Particles)
        {
            int index = (int)position.X + (int)position.Y * _canvasSize.Width;
            if (index >= 0 && index < maxIndex)
            {
                pixels[index] = particle.GetColor();
            }
        }
    }

    private void OnTouch(object sender, SKTouchEventArgs args)
    {
        if (args.ActionType == SKTouchAction.Moved)
        {
            _cursor.X = (int)(args.Location.X / _canvasScale.X);
            _cursor.Y = (int)(args.Location.Y / _canvasScale.Y);
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

    private void OnTogglePlayPage(object sender, EventArgs e)
    {
        throw new NotImplementedException("TODO - simulation play pause");
    }

    private async void OnOpenUploadPage(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(_uploadPage);
    }

    private void OnOpenDownloadPage(object sender, EventArgs e)
    {
        throw new NotImplementedException("TODO - download page");
    }

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas.InvalidateSurface());
    }
}