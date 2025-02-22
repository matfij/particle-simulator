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
    private readonly System.Timers.Timer PaintTimer = new(20); // 1000ms / 20 = 50fps
    private readonly SKBitmap ParticlesBitmap = new(CanvasSize.Width, CanvasSize.Height);

    private (float X, float Y, float R) Cursor = (0, 0, 10);

    public MainPage()
    {
        InitializeComponent();
        ParticlesManager = new ParticlesManager();
        PaintTimer.Elapsed += (sender, args) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        PaintTimer.Start();
    }

    private unsafe void UpdateBitmap()
    {
        ParticlesBitmap.Erase(SKColors.Black);
        var pixels = (uint*)ParticlesBitmap.GetPixels();
        var maxIndex = CanvasSize.Width * CanvasSize.Height;

        foreach (var particle in ParticlesManager.GetParticles)
        {
            int index = particle.X + particle.Y * CanvasSize.Width;
            if (index >= 0 && index < maxIndex)
            {
                pixels[index] = Particle.Color;
            }
        }
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        UpdateBitmap();

        var canvas = args.Surface.Canvas;
        canvas.Clear(SKColors.Black);

        var targetRect = new SKRect(0, 0, args.Info.Width, args.Info.Height);
        canvas.DrawBitmap(ParticlesBitmap, targetRect);
        canvas.DrawCircle(Cursor.X, Cursor.Y, Cursor.R, CursorPaint);
    }

    private void OnTouch(object sender, SKTouchEventArgs args)
    {
        if (args.ActionType == SKTouchAction.Moved)
        {
            Cursor.X = args.Location.X;
            Cursor.Y = args.Location.Y;
        }
        if (args.ActionType == SKTouchAction.WheelChanged)
        {
            var radius = (int)(Cursor.R + args.WheelDelta / 10);
            Cursor.R = Math.Clamp(radius, 1, 100);
        }
        if (args.ActionType == SKTouchAction.Pressed || args.ActionType == SKTouchAction.Moved)
        {
            var centerX = (int)args.Location.X;
            var centerY = (int)args.Location.Y;
        }
    }

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas.InvalidateSurface());
    }
}


