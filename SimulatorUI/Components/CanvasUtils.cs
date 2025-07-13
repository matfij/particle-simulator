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
        Color = 0x99F8F8FF,
        Style = SKPaintStyle.Stroke,
    };
    public static readonly SKPaint _fontPaint = new()
    {
        StrokeWidth = 1,
        IsAntialias = true,
        Color = 0xDDF8F8FF,
        Style = SKPaintStyle.StrokeAndFill,
    };
    public static readonly SKFont _font = new()
    {
        Edging = SKFontEdging.SubpixelAntialias,
    };

    public static (float, float) GetScale(float width, float height)
        => (width / (float)CanvasUtils._canvasSize.Width, height / (float)CanvasUtils._canvasSize.Height);

    public static (string label, Vector2 postion, SKTextAlign align) GetMarkedParticleInfo(Vector2 basePosition, Particle? particle)
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
            postion = new Vector2(x, y),
            align = align,
        };
    }
}
