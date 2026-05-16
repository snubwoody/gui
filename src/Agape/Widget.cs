using System.Numerics;
using SkiaSharp;

namespace Agape;

public static class LayoutSolver {
    public static void SolveLayout(RenderObject renderObject, double width, double height) {
        if (renderObject.IntrinsicWidth is BoxSizing.Fixed fixedWidth) {
            renderObject.Constraints.MaximumWidth = fixedWidth.Value;
        } else {
            renderObject.Constraints.MaximumWidth ??= width;
        }

        if (renderObject.IntrinsicHeight is BoxSizing.Fixed fixedHeight) {
            renderObject.Constraints.MaximumHeight = fixedHeight.Value;
        } else {
            renderObject.Constraints.MaximumHeight ??= height;
        }

        // It's important that the min constraints are solved before the max constraints
        // because the min constraints are used in calculating max constraints.
        renderObject.SolveMinConstraints();
        renderObject.SolveMaxConstraints();

        renderObject.UpdateSize();
        renderObject.PositionChildren();
    }
}

public enum AxisAlignment {
    Start,
    Center,
    End
}

/// <summary>
/// The space between a widget's boundary and it's content.
/// </summary>
public record struct Padding {
    public double Left { get; set; }
    public double Right { get; set; }
    public double Top { get; set; }
    public double Bottom { get; set; }

    public Padding(double left = 0, double right = 0, double top = 0, double bottom = 0) {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    /// <summary>
    /// Creates a <see cref="Padding"/> where all sides have the same value.
    /// </summary>
    public static Padding All(double value) => new(value, value, value, value);

    public static Padding Symmetric(double horizontal, double vertical) =>
        new(left: horizontal, right: horizontal, top: vertical, bottom: vertical);

    /// <summary>
    /// Returns the sum of the left and right padding.
    /// </summary>
    public double SumHorizontal() => Left + Right;

    /// <summary>
    /// Returns the sum of the top and bottom padding.
    /// </summary>
    public double SumVertical() => Top + Bottom;
}

public abstract record BoxSizing {
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

public record BoxConstraints {
    /// <summary>
    /// The minimum width a height is allowed to be, takes precedence over all values.  
    /// </summary>
    public double? MinimumWidth { get; set; }
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
        ) {
        MinimumWidth = minWidth;
        MinimumHeight = minHeight;
        MaximumWidth = maxWidth;
        MaximumHeight = maxHeight;
    }
}

// TODO: add render widget
public abstract class Widget {
    public BoxSizing IntrinsicWidth { get; init; } = new BoxSizing.Shrink();
    public BoxSizing IntrinsicHeight { get; init; } = new BoxSizing.Shrink();
    public BoxConstraints Constraints { get; init; } = new();

    public abstract RenderObject CreateRenderObject();
}


public class Container : Widget {
    public SKColor Color { get; set; }

    Widget _child;

    public Container(Widget child) {
        _child = child;
    }

    public override RenderObject CreateRenderObject() {
        return new SingleChildRenderObject(child: _child.CreateRenderObject()) {
            Color = Color,
            IntrinsicWidth = IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints
        };
    }
}

public class Rect : Widget {
    public SKColor Color { get; set; }

    public override RenderObject CreateRenderObject() {
        return new EmptyRenderObject {
            Color = Color,
            IntrinsicWidth = IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints
        };
    }
}

