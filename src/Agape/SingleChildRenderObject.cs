using SkiaSharp;

namespace Agape;

/// <summary>
/// A widget with one child.
/// </summary>
public class SingleChildRenderObject : RenderObject {
    public RenderObject Child { get; }
    public SKColor Color { get; set; }

    public SingleChildRenderObject(RenderObject child) {
        Child = child;
    }

    public override void SolveMinConstraints() {
        Child.SolveMinConstraints();
        // TODO get the max or use null expr
        if (IntrinsicWidth is BoxSizing.Fixed width) {
            Constraints.MinimumWidth = width.Value;
        } else {
            var minWidth = Math.Max(Child.Constraints.MinimumWidth ?? 0, Constraints.MinimumWidth ?? 0);
            Constraints.MinimumWidth = minWidth + Padding.SumHorizontal();
        }

        if (IntrinsicHeight is BoxSizing.Fixed height) {
            Constraints.MinimumHeight = height.Value;
        } else {
            var minHeight = Math.Max(Child.Constraints.MinimumHeight ?? 0, Constraints.MinimumHeight ?? 0);
            Constraints.MinimumHeight = minHeight + Padding.SumVertical();
        }
    }

    public override void SolveMaxConstraints() {
        var availableWidth = Constraints.MaximumWidth ?? 0;
        var availableHeight = Constraints.MaximumHeight ?? 0;

        availableWidth -= Padding.SumHorizontal();
        availableHeight -= Padding.SumVertical();

        // TODO: should layout set max constraints when shrink?
        if (Child.IntrinsicWidth is BoxSizing.Fill) {
            if (!Child.Constraints.MaximumWidth.HasValue) {
                Child.Constraints.MaximumWidth = availableWidth;
            }

        } else if (Child.IntrinsicWidth is BoxSizing.Fixed fixedWidth) {
            Child.Constraints.MaximumWidth = fixedWidth.Value;
        }

        if (Child.IntrinsicHeight is BoxSizing.Fill) {
            if (!Child.Constraints.MaximumHeight.HasValue) {
                Child.Constraints.MaximumHeight = availableHeight;
            }

        } else if (Child.IntrinsicHeight is BoxSizing.Fixed fixedHeight) {
            Child.Constraints.MaximumHeight = fixedHeight.Value;
        }

    }

    private void AlignMainAxisStart() {
        var xPos = Position.X;
        xPos += (float)Padding.Left;
        
        Child.Position = Child.Position with {
            X = xPos
        };
    }
    
    private void AlignMainAxisCenter() {
        var centerStart = Position.X + (Width - Child.Width) / 2;
        Child.Position = Child.Position with {
            X = (float)centerStart
        };
    }

    private void AlignMainAxisEnd() {
        // let mut x_pos = self.position.x + self.size.width;
        // x_pos -= self.padding.right;
        var xPos = Position.X + Width;
        xPos -= Padding.Left;
        
        Child.Position = Child.Position with {
            X = (float)xPos
        };
    }
    
    public override void PositionChildren() {
        switch (MainAxisAlignment) {
            case AxisAlignment.Start:
                AlignMainAxisStart();
                break;
            case AxisAlignment.Center:
                AlignMainAxisCenter();
                break;
            case AxisAlignment.End:
                AlignMainAxisEnd();
                break;
        }
    }
    

    public override void Draw(SKCanvas canvas) {
        var rect = SKRect.Create(0, 0, (float)Width, (float)Height);
        var paint = new SKPaint {
            Color = Color,
        };
        canvas.DrawRect(rect, paint);
        Child.Draw(canvas);
    }
}