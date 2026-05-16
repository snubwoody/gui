using System.Numerics;
using SkiaSharp;

namespace Agape;

public class TextRenderObject : RenderObject {
    public string Value { get; init; }
    /// <summary>
    /// The font size of the text.
    /// </summary>
    public double Size { get; init; } = 16;

    public TextRenderObject(string value) {
        Value = value;
    }

    /// <summary>
    /// Measure the bounds of the text.
    /// </summary>
    public Vector2 MeasureText() {
        Font().MeasureText(Value, out var bounds, Paint());
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

    public override void SolveMinConstraints() {
        var bounds = MeasureText();
        // The minimum width and height take precedence if set
        if (IntrinsicWidth is BoxSizing.Fixed width && !Constraints.MinimumWidth.HasValue) {
            Constraints.MinimumWidth = width.Value;
        } else if (IntrinsicWidth is BoxSizing.Shrink) {
            Constraints.MinimumWidth = bounds.X;
        }

        if (IntrinsicHeight is BoxSizing.Fixed height && !Constraints.MinimumHeight.HasValue) {
            Constraints.MinimumHeight = height.Value;
        } else if (IntrinsicHeight is BoxSizing.Shrink) {
            Constraints.MinimumHeight = bounds.Y;
        }
    }


    public override void SolveMaxConstraints() { }

    public override void PositionChildren() { }


    public override void Draw(SKCanvas canvas) {
        var paint = Paint();
        var font = Font();

        // Draw at the baseline
        var y = Position.Y + Height;
        canvas.DrawText(Value, Position.X, (float)y, font, paint);
    }
}