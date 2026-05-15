using SkiaSharp;

namespace Agape;

/// <summary>
/// A render object with no children.
/// </summary>
public class EmptyRenderObject : RenderObject {
    public SKColor Color { get; set; }

    public override void SolveMinConstraints() {
        // The minimum width and height take precedence if set
        if (IntrinsicWidth is BoxSizing.Fixed width && !Constraints.MinimumWidth.HasValue) {
            Constraints.MinimumWidth = width.Value;
        }

        if (IntrinsicHeight is BoxSizing.Fixed height && !Constraints.MinimumHeight.HasValue) {
            Constraints.MinimumHeight = height.Value;
        }
    }


    public override void SolveMaxConstraints() {
        // No children to solve for    
    }

    public override void Draw(SKCanvas canvas) {
        var rect = SKRect.Create(0, 0, (float)Width, (float)Height);
        var paint = new SKPaint {
            Color = Color,
        };
        canvas.DrawRect(rect, paint);
    }
}