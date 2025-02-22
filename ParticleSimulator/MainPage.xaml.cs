using Microsoft.Maui.Controls.PlatformConfiguration;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ParticleSimulator;

public partial class MainPage : ContentPage
{
    private static readonly (int Width, int Height) CanvasSize = (1200, 600);
    private readonly SKPaint CursorPaint = new()
    {
        Color = SKColors.GhostWhite,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 2
    };
    private readonly SKBitmap ParticlesBitmap = new(CanvasSize.Width, CanvasSize.Height);
    private readonly System.Timers.Timer PaintTimer = new(20); // 1000ms / 20 = 50fps
    private (float X, float Y) CanvasScale = (1, 1);
    private (float X, float Y, float R) Cursor = (0, 0, 10);

    public MainPage()
    {
        InitializeComponent();
        PaintTimer.Elapsed += (sender, args) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        PaintTimer.Start();
    }

    private void UpdateBitmap()
    {
        ParticlesBitmap.Erase(SKColors.Black);
        //var pixels = (uint*)ParticlesBitmap.GetPixels();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        CanvasScale = (args.Info.Width / CanvasSize.Width, args.Info.Height / CanvasSize.Height);
        UpdateBitmap();

        var canvas = args.Surface.Canvas;
        canvas.Clear(SKColors.Black);
        canvas.Scale((CanvasScale.X + CanvasScale.Y) / 2);

        canvas.DrawBitmap(ParticlesBitmap, 0, 0);
        canvas.DrawCircle(Cursor.X, Cursor.Y, Cursor.R, CursorPaint);
    }

    private void OnTouch(object sender, SKTouchEventArgs args)
    {
        if (args.ActionType == SKTouchAction.Moved)
        {
            Cursor.X = args.Location.X / CanvasScale.X;
            Cursor.Y = args.Location.Y / CanvasScale.Y;
        }
        if (args.ActionType == SKTouchAction.WheelChanged)
        {
            var radius = (int)(2 * (Cursor.R + args.WheelDelta / 10) / (CanvasScale.X + CanvasScale.Y));
            Cursor.R = Math.Clamp(radius, 1, 100);
        }
        if ((args.ActionType == SKTouchAction.Pressed || args.ActionType == SKTouchAction.Moved))
        {
            var centerX = (int)(args.Location.X / CanvasScale.X);
            var centerY = (int)(args.Location.Y / CanvasScale.Y);
        }
    }

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas.InvalidateSurface());
    }
}


