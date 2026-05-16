using SkiaSharp;

namespace Agape;

/// <summary>
/// A widget with one child.
/// </summary>
public class SingleChildRenderObject : RenderObject {
    private RenderObject _child;
    public SKColor Color { get; set; }

    public SingleChildRenderObject(RenderObject child) {
        _child = child;
    }

    public override void SolveMinConstraints() {
        _child.SolveMinConstraints();
        // TODO get the max or use null expr
        if (IntrinsicWidth is BoxSizing.Fixed width) {
            Constraints.MinimumWidth = width.Value;
        } else {
            var minWidth = Math.Max(_child.Constraints.MinimumWidth ?? 0, Constraints.MinimumWidth ?? 0);
            Constraints.MinimumWidth = minWidth + Padding.SumHorizontal();
        }

        if (IntrinsicHeight is BoxSizing.Fixed height) {
            Constraints.MinimumHeight = height.Value;
        } else {
            var minHeight = Math.Max(_child.Constraints.MinimumHeight ?? 0, Constraints.MinimumHeight ?? 0);
            Constraints.MinimumHeight = minHeight + Padding.SumVertical();
        }
    }

    public override void SolveMaxConstraints() {
        var availableWidth = Constraints.MaximumWidth ?? 0;
        var availableHeight = Constraints.MaximumHeight ?? 0;

        availableWidth -= Padding.SumHorizontal();
        availableHeight -= Padding.SumVertical();

        // TODO: should layout set max constraints when shrink?
        if (_child.IntrinsicWidth is BoxSizing.Fill) {
            if (!_child.Constraints.MaximumWidth.HasValue) {
                _child.Constraints.MaximumWidth = availableWidth;
            }

        } else if (_child.IntrinsicWidth is BoxSizing.Fixed fixedWidth) {
            _child.Constraints.MaximumWidth = fixedWidth.Value;
        }

        if (_child.IntrinsicHeight is BoxSizing.Fill) {
            if (!_child.Constraints.MaximumHeight.HasValue) {
                _child.Constraints.MaximumHeight = availableHeight;
            }

        } else if (_child.IntrinsicHeight is BoxSizing.Fixed fixedHeight) {
            _child.Constraints.MaximumHeight = fixedHeight.Value;
        }

    }

    public override void PositionChildren() {
        
    }
    

    public override void Draw(SKCanvas canvas) {
        var rect = SKRect.Create(0, 0, (float)Width, (float)Height);
        var paint = new SKPaint {
            Color = Color,
        };
        canvas.DrawRect(rect, paint);
        _child.Draw(canvas);
    }
}