using System.Diagnostics;
using SimulatorEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SimulatorUI;

public partial class MainPage : ContentPage
{
    private static readonly (int Width, int Height) CanvasSize = (1200, 600);
    private static readonly SKPaint CursorPaint = new()
    {
        StrokeWidth = 2,
        IsAntialias = true,
        Color = SKColors.GhostWhite,
        Style = SKPaintStyle.Stroke,
    };
    private readonly ParticlesManager ParticlesManager;
    private readonly System.Timers.Timer PaintTimer = new(20);
    private readonly System.Timers.Timer PrintTimer = new(200);
    private readonly SKBitmap ParticlesBitmap = new(CanvasSize.Width, CanvasSize.Height);
    private (float X, float Y) CanvasScale = (1, 1);
    private (float X, float Y, float R) Cursor = (0, 0, 10);
    private ParticleKind CurrentParticleKind = ParticleKind.Water;
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _paintTime = new();

    public MainPage()
    {
        InitializeComponent();
        ParticlesManager = new ParticlesManager();
        PaintTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        PaintTimer.Start();
        PrintTimer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(PrintPerformanceInfo);
        PrintTimer.Start();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        _stopwatch.Restart();
        try
        {
            CanvasScale = (args.Info.Width / (float)CanvasSize.Width, args.Info.Height / (float)CanvasSize.Height);
            UpdateBitmap();
            var canvas = args.Surface.Canvas;
            canvas.Clear(SKColors.Black);
            canvas.Scale(CanvasScale.X, CanvasScale.Y);

            canvas.DrawBitmap(ParticlesBitmap, 0, 0);
            canvas.DrawCircle(Cursor.X, Cursor.Y, Cursor.R, CursorPaint);
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
        ParticleCountLabel.Text = $"Particles: {ParticlesManager.GetParticlesCount}";
        ComputeTimeLabel.Text = $"Compute time: {(int)ParticlesManager.LoopTime.TotalMilliseconds} [ms]";
        PaintTimeLabel.Text = $"Paint time: {(int)_paintTime.TotalMilliseconds} [ms]";
    }

    private unsafe void UpdateBitmap()
    {
        ParticlesBitmap.Erase(SKColors.Black);
        var pixels = (uint*)ParticlesBitmap.GetPixels();
        var maxIndex = CanvasSize.Width * CanvasSize.Height;

        foreach (var (position, particle) in ParticlesManager.GetParticles)
        {
            int index = (int)position.X + (int)position.Y * CanvasSize.Width;
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
            Cursor.X = (int)(args.Location.X / CanvasScale.X);
            Cursor.Y = (int)(args.Location.Y / CanvasScale.Y);
        }
        if (args.ActionType == SKTouchAction.WheelChanged)
        {
            var radius = (int)(Cursor.R + args.WheelDelta / 25);
            Cursor.R = Math.Clamp(radius, 1, 100);
        }
        if (args.ActionType == SKTouchAction.Pressed || args.ActionType == SKTouchAction.Moved)
        {
            if (args.MouseButton == SKMouseButton.Left)
            {
                ParticlesManager.AddParticles(new(Cursor.X, Cursor.Y), (int)Cursor.R, CurrentParticleKind);
            }
            if (args.MouseButton == SKMouseButton.Right)
            {
                ParticlesManager.RemoveParticles(new(Cursor.X, Cursor.Y), (int)Cursor.R);
            }
        }
    }

    private void SetParticleKind(object sender, EventArgs e)
    {
        if (sender is Button selectedButton && selectedButton.CommandParameter is ParticleKind kind)
        {
            CurrentParticleKind = kind;

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

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas.InvalidateSurface());
    }
}