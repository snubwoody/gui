using System.Numerics;
using SkiaSharp;

namespace Agape;

public abstract class Widget
{
    /// <summary>
    /// Draw the widget to the skia canvas.
    /// </summary>
    public abstract void Draw(SKCanvas canvas);
}

public class Rect: Widget
{
    public Vector2 Size { get; set; }
    public SKColor Color { get; set; }

    public override void Draw(SKCanvas canvas)
    {
    
        var rect = SKRect.Create(80, 80, Size.X, Size.Y);
        var paint = new SKPaint
        {
            Color = Color,
        };
        canvas.DrawRect(rect,paint);
    }
}