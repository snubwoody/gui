using System.Numerics;
using SkiaSharp;

namespace Agape;

public abstract record BoxSizing
{
    private BoxSizing() { }
    public sealed record Fixed(double Value) : BoxSizing;
    /// <summary>
    /// Fits it children, i.e. as small as possible.
    /// </summary>
    public sealed record Fit : BoxSizing;
    /// <summary>
    /// Fills the available width.
    /// </summary>
    /// <param name="Factor"></param>
    public sealed record Flex(double Factor) : BoxSizing;
}

public record BoxConstraints
{
    /// <summary>
    /// The minimum width a height is allowed to be, takes precedence over all values.  
    /// </summary>
    public double? MinimumWidth { get; set; } = null;
    /// <summary>
    /// The minimum height a widget is allowed to be, takes precedence over all values.  
    /// </summary>
    public double? MinimumHeight { get; set; }
    public double? MaximumWidth { get; set; }
    public double? MaximumHeight { get; set; }

    public BoxConstraints(
        double? minWidth = null,
        double? minHeight = null,
        double? maxWidth = null,
        double? maxHeight = null
        )
    {
        MinimumWidth = minWidth;
        MinimumHeight = minHeight;
        MaximumWidth = maxWidth;
        MaximumHeight = maxHeight;
    }
}

public abstract class Widget
{
    public BoxSizing IntrinsicWidth { get; init; }
    public BoxSizing IntrinsicHeight { get; init; }
    public BoxConstraints Constraints { get; init; } = new();
    /// <summary>
    /// The minimum width a widget is allowed to be, takes precedence over all values.  
    /// </summary>
    public double? MinWidth { get; init; }
    /// <summary>
    /// The minimum width a height is allowed to be, takes precedence over all values.  
    /// </summary>
    public double? MinHeight { get; init; }

    public void SolveMinConstraints()
    {
        // The minimum width and height take precedence if set
        if (IntrinsicWidth is BoxSizing.Fixed value && !Constraints.MinimumWidth.HasValue)
        {
            Constraints.MinimumWidth = value.Value;
        }

        if (IntrinsicHeight is BoxSizing.Fixed fixedHeight && !Constraints.MinimumHeight.HasValue)
        {
            Constraints.MinimumHeight = fixedHeight.Value;
        }

    }

    /// <summary>
    /// Draw the widget to the skia canvas.
    /// </summary>
    public abstract void Draw(SKCanvas canvas);
}

public class Rect : Widget
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
        canvas.DrawRect(rect, paint);
    }
}