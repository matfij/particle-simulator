﻿using Microsoft.Maui.Controls.Compatibility;
using SimulatorEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SimulatorUI;

public partial class MainPage : ContentPage
{
    private static readonly (int Width, int Height) CanvasSize = (1800, 900);
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
    private ParticleKind ParticleToAdd = ParticleKind.Sand;

    public MainPage()
    {
        InitializeComponent();
        ParticlesManager = new ParticlesManager();
        PaintTimer.Elapsed += (sender, args) => MainThread.BeginInvokeOnMainThread(InvalidateCanvas);
        PaintTimer.Start();
    }

    private void SetParticleKind(object sender, EventArgs e)
    {
        if (sender is Button selectedButton && selectedButton.CommandParameter is ParticleKind kind)
        {
            ParticleToAdd = kind;

            if (selectedButton.Parent is HorizontalStackLayout parentLayout)
            {
                foreach (var child in parentLayout.Children)
                {
                    if (child is Button button)
                    {
                        button.Opacity = 0.9;
                        button.BorderWidth = 1;
                        selectedButton.FontAttributes = FontAttributes.None;
                    }
                }
            }

            selectedButton.Opacity = 1;
            selectedButton.BorderWidth = 2;
            selectedButton.FontAttributes = FontAttributes.Bold;
        }
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
                pixels[index] = particle.GetColor();
            }
        }
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        UpdateBitmap();

        var canvas = args.Surface.Canvas;
        canvas.Clear(SKColors.Black);

        canvas.DrawBitmap(ParticlesBitmap, 0, 0);
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
            if (args.MouseButton == SKMouseButton.Left)
            {
                ParticlesManager.AddParticles(((int)Cursor.X, (int)Cursor.Y), (int)Cursor.R, ParticleToAdd);
                ParticleCountLabel.Text = $"Particles: {ParticlesManager.GetParticlesCount}";
            }
            if (args.MouseButton == SKMouseButton.Right)
            {
                // TODO - erase particles
            }
        }
    }

    private void InvalidateCanvas()
    {
        MainThread.BeginInvokeOnMainThread(() => ParticleCanvas.InvalidateSurface());
    }
}


