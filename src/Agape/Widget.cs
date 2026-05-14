using System.Numerics;
using SkiaSharp;

namespace Agape;

/// <summary>
/// The space between a widget's boundary and it's content.
/// </summary>
public record struct Padding
{
    public double Left { get; set; }
    public double Right { get; set; }
    public double Top { get; set; }
    public double Bottom { get; set; }

    public Padding(double left, double right, double top, double bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
    
    /// <summary>
    /// Creates a <see cref="Padding"/> where all sides have the same value.
    /// </summary>
    public static Padding All(double value) => new (value,value,value,value);

    public static Padding Symmetric(double horizontal, double vertical) =>
        new (left: horizontal, right: horizontal, top: vertical, bottom: vertical);

    public double SumHorizontal() => Left + Right;
    public double SumVertical() => Top + Bottom;
}

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

// TODO: add render widget
public abstract class Widget
{
    public BoxSizing IntrinsicWidth { get; init; } = new BoxSizing.Shrink();
    public BoxSizing IntrinsicHeight { get; init; } = new BoxSizing.Shrink();
    public BoxConstraints Constraints { get; init; } = new();
    public Padding Padding { get; init; } = new();

    /// <summary>
    /// Solves the minimum constraints. The children widgets tell the parent the minimum space they need.
    /// </summary>
    public abstract void SolveMinConstraints();
    
    /// <summary>
    /// Solves the maximum constraints. The parent widget tells the children the maximum space available to
    /// them.
    /// </summary>
    public abstract void SolveMaxConstraints();

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
    
    
    public override void SolveMaxConstraints()
    {
        // No children to solve for    
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
                var minWidth = Math.Max(_child.Constraints.MinimumWidth ?? 0,Constraints.MinimumWidth ?? 0);
                Constraints.MinimumWidth = minWidth + Padding.SumHorizontal();
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
                var minHeight = Math.Max(_child.Constraints.MinimumHeight ?? 0,Constraints.MinimumHeight ?? 0);
                Constraints.MinimumHeight = minHeight + Padding.SumVertical();
            }
        }
    }

    public override void SolveMaxConstraints()
    {
        var availableWidth = Constraints.MaximumWidth ?? 0;
        var availableHeight = Constraints.MaximumHeight ?? 0;
        
        availableWidth -= Padding.SumHorizontal();
        availableHeight -= Padding.SumVertical();

        // TODO: should layout set max constraints when shrink?
        if (_child.IntrinsicWidth is BoxSizing.Fill)
        {
            if (!_child.Constraints.MaximumWidth.HasValue)
            {
                _child.Constraints.MaximumWidth = availableWidth;
            }
            
        } else if (_child.IntrinsicWidth is BoxSizing.Fixed fixedWidth)
        {
            _child.Constraints.MaximumWidth = fixedWidth.Value;
        }

        if (_child.IntrinsicHeight is BoxSizing.Fill)
        {
            if (!_child.Constraints.MaximumHeight.HasValue)
            {
                _child.Constraints.MaximumHeight = availableHeight;
            }
            
        } else if (_child.IntrinsicHeight is BoxSizing.Fixed fixedHeight)
        {
            _child.Constraints.MaximumHeight = fixedHeight.Value;
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