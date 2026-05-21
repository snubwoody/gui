using System.Text.Json;
using SkiaSharp;

namespace Agape;

/// <summary>
/// A widget with one child.
/// </summary>
public class SingleChildRenderObject : RenderObject {
    public RenderObject Child { get; }
    public Color Color { get; set; }

    public SingleChildRenderObject(RenderObject child) {
        Child = child;
    }

    public override void UpdateSize() {
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

        Child.UpdateSize();
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

        Child.SolveMaxConstraints();
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
        var xPos = Position.X + Width;
        xPos -= Padding.Left;
        xPos -= Child.Width;

        Child.Position = Child.Position with {
            X = (float)xPos
        };
    }


    private void AlignCrossAxisStart() {
        var yPos = Position.Y + Padding.Top;

        Child.Position = Child.Position with {
            Y = (float)yPos
        };
    }

    private void AlignCrossAxisCenter() {
        var centerStart = Position.Y + (Height - Child.Height) / 2;
        Child.Position = Child.Position with {
            Y = (float)centerStart
        };
    }


    private void AlignCrossAxisEnd() {
        var yPos = Position.Y + Height - Padding.Bottom - Child.Height;

        Child.Position = Child.Position with {
            Y = (float)yPos
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

        switch (CrossAxisAlignment) {
            case AxisAlignment.Start:
                AlignCrossAxisStart();
                break;
            case AxisAlignment.Center:
                AlignCrossAxisCenter();
                break;
            case AxisAlignment.End:
                AlignCrossAxisEnd();
                break;
        }
    }

    public override void Draw(SKCanvas canvas) {
        var rect = SKRect.Create(Position.X, Position.Y, (float)Width, (float)Height);
        var paint = new SKPaint {
            Color = Color.ToSKColor(),
        };
        canvas.DrawRect(rect, paint);
        Child.Draw(canvas);
    }
}