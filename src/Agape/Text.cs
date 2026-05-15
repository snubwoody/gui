using System.Drawing;
using System.Numerics;
using SkiaSharp;

namespace Agape;

public class Text {
    public string Value { get; init; }
    /// <summary>
    /// The font size of the text.
    /// </summary>
    public double Size { get; init; } = 16;

    public Text(string value) {
        Value = value;
    }

    /// <summary>
    /// Measure the size of the text.
    /// </summary>
    public Vector2 MeasureText() {
        var bounds = new SKRect();
        Font().MeasureText(Value, out bounds);
        return new Vector2(bounds.Width, bounds.Height);
    }
    
    private SKFont Font() {
        return new SKFont {
            Size = (float)Size
        };
    }
    
    /// <summary>
    /// Create a paint for the text.
    /// </summary>
    private SKPaint Paint() {
        return new SKPaint {
            Color = SKColors.Black,
            IsAntialias = true
        };
    }
    
    public void Draw(SKCanvas canvas) {
        var paint = Paint();
        var font = Font();
        // var position = Position;
        // Draw at the baseline
        // var y = Position.Y + Size.Y;
        canvas.DrawText(Value, 0, 0, font, paint);
    }
}

