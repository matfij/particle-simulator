using System.Numerics;
using SimulatorEngine.Particles;
using SimulatorUI.Resources.Locales;
using SkiaSharp;

namespace SimulatorUI.Components;

internal static class CanvasUtils
{
    public static readonly (int Width, int Height) _canvasSize = (1200, 600);
    public static readonly SKPaint _cursorPaint = new()
    {
        StrokeWidth = 2,
        IsAntialias = true,
        Color = SKColors.GhostWhite.WithAlpha(150),
        Style = SKPaintStyle.Stroke,
    };
    public static readonly SKPaint _fontPaint = new()
    {
        StrokeWidth = 1,
        IsAntialias = true,
        Color = SKColors.GhostWhite.WithAlpha(225),
        Style = SKPaintStyle.StrokeAndFill,
        ImageFilter = SKImageFilter.CreateDropShadow(
            dx: 1,
            dy: 1, 
            sigmaX: 2,
            sigmaY: 2,
            color: SKColors.Black
        )
    };
    public static readonly SKFont _font = new()
    {
        Edging = SKFontEdging.SubpixelAntialias,
    };

    public static (float, float) GetScale(float width, float height)
        => (width / _canvasSize.Width, height / _canvasSize.Height);

    public static (string label, Vector2 position, SKTextAlign align) GetMarkedParticleInfo(Vector2 basePosition, Particle? particle)
    {
        var label =
            particle is not null
            ? $"{particle.Kind}, {String.Format(AppStrings.Temperature, particle.Temperature)}"
            : AppStrings.Empty;

        var x = basePosition.X + 10;
        var y = basePosition.Y - 10;
        var align = SKTextAlign.Left;

        if (y < 50)
        {
            y += 30;
        }
        if (x > 1150)
        {
            x -= 20;
            align = SKTextAlign.Right;
        }

        return new ()
        {
            label = label,
            position = new Vector2(x, y),
            align = align,
        };
    }
}
