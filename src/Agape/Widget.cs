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
    public sealed record Shrink : BoxSizing;
    /// <summary>
    /// Fills the available width.
    /// </summary>
    /// <param name="Factor"></param>
    public sealed record Fill(double Factor) : BoxSizing;
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
    public BoxSizing IntrinsicWidth { get; init; } = new BoxSizing.Shrink();
    public BoxSizing IntrinsicHeight { get; init; } = new BoxSizing.Shrink();
    public BoxConstraints Constraints { get; init; } = new();

    public abstract void SolveMinConstraints();

    /// <summary>
    /// Draw the widget to the skia canvas.
    /// </summary>
    public abstract void Draw(SKCanvas canvas);
}

/// <summary>
/// A widget with no children.
/// </summary>
public abstract class EmptyWidget : Widget
{
    public override void SolveMinConstraints()
    {
        // The minimum width and height take precedence if set
        if (IntrinsicWidth is BoxSizing.Fixed width && !Constraints.MinimumWidth.HasValue)
        {
            Constraints.MinimumWidth = width.Value;
        }

        if (IntrinsicHeight is BoxSizing.Fixed height && !Constraints.MinimumHeight.HasValue)
        {
            Constraints.MinimumHeight = height.Value;
        }
    }
}

/// <summary>
/// A widget with one child.
/// </summary>
public abstract class SingleChildWidget : Widget
{
    protected Widget _child;

    public SingleChildWidget(Widget child)
    {
        _child = child;
    }
    
    public override void SolveMinConstraints()
    {
        _child.SolveMinConstraints();
        if (!Constraints.MinimumWidth.HasValue)
        {
            if (IntrinsicWidth is BoxSizing.Fixed width)
            {
                Constraints.MinimumWidth = width.Value;
            }
            else
            {
                Constraints.MinimumWidth = _child.Constraints.MinimumWidth;
            }
        }

        if (!Constraints.MinimumHeight.HasValue)
        {
            if (IntrinsicHeight is BoxSizing.Fixed height)
            {
                Constraints.MinimumHeight = height.Value;
            }
            else
            {
                Constraints.MinimumHeight = _child.Constraints.MinimumHeight;
            }
        }
    }
}

public class Container : SingleChildWidget
{
    public Vector2 Size { get; set; }
    public SKColor Color { get; set; }

    public Container(Widget child) : base(child){}

    public override void Draw(SKCanvas canvas)
    {

        var rect = SKRect.Create(80, 80, Size.X, Size.Y);
        var paint = new SKPaint
        {
            Color = Color,
        };
        canvas.DrawRect(rect, paint);
        _child.Draw(canvas);
    }
}

public class Rect : EmptyWidget
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