using System.Numerics;
using SkiaSharp;

namespace Agape;

public abstract class RenderObject {
    public Vector2 Position { get; set; }
    public AxisAlignment MainAxisAlignment { get; init; } = AxisAlignment.Start;
    public AxisAlignment CrossAxisAlignment { get; init; } = AxisAlignment.Start;
    public double Width { get; set; }
    public double Height { get; set; }

    public BoxSizing IntrinsicWidth { get; init; } = new BoxSizing.Shrink();
    public BoxSizing IntrinsicHeight { get; init; } = new BoxSizing.Shrink();
    public BoxConstraints Constraints { get; init; } = new();

    public Padding Padding { get; set; } = new();

    /// <summary>
    /// Solves the minimum constraints. The children widgets tell the parent the minimum space they need.
    /// </summary>
    public abstract void SolveMinConstraints();

    /// <summary>
    /// Solves the maximum constraints. The parent widget tells the children the maximum space available to
    /// them.
    /// </summary>
    public abstract void SolveMaxConstraints();
    
    public abstract void PositionChildren();

    /// <summary>
    /// Sets the widget's size based on the constraints and box sizing.
    /// </summary>
    public void UpdateSize() {
        Width = IntrinsicWidth switch {
            BoxSizing.Fixed fixedWidth => fixedWidth.Value,
            BoxSizing.Fill => Constraints.MaximumWidth ?? 0,
            BoxSizing.Shrink => Constraints.MinimumHeight ?? 0,
            _ => throw new Exception("Invalid intrinsic width")
        };

        Height = IntrinsicHeight switch {
            BoxSizing.Fixed fixedHeight => fixedHeight.Value,
            BoxSizing.Fill => Constraints.MaximumHeight ?? 0,
            BoxSizing.Shrink => Constraints.MinimumHeight ?? 0,
            _ => throw new Exception("Invalid intrinsic height")
        };
    }

    /// <summary>
    /// Draw the render object to the skia canvas.
    /// </summary>
    public abstract void Draw(SKCanvas canvas);
}