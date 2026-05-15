using System.Numerics;
using SkiaSharp;

namespace Agape;

public abstract class RenderObject {
    public double Width { get; set; }
    public double Height { get; set; }

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
    /// Draw the widget to the skia canvas.
    /// </summary>
    public abstract void Draw(SKCanvas canvas);
}

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
        if (!Constraints.MinimumWidth.HasValue) {
            if (IntrinsicWidth is BoxSizing.Fixed width) {
                Constraints.MinimumWidth = width.Value;
            } else {
                var minWidth = Math.Max(_child.Constraints.MinimumWidth ?? 0, Constraints.MinimumWidth ?? 0);
                Constraints.MinimumWidth = minWidth + Padding.SumHorizontal();
            }
        }

        if (!Constraints.MinimumHeight.HasValue) {
            if (IntrinsicHeight is BoxSizing.Fixed height) {
                Constraints.MinimumHeight = height.Value;
            } else {
                var minHeight = Math.Max(_child.Constraints.MinimumHeight ?? 0, Constraints.MinimumHeight ?? 0);
                Constraints.MinimumHeight = minHeight + Padding.SumVertical();
            }
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

    public override void Draw(SKCanvas canvas) {
        var rect = SKRect.Create(0, 0, (float)Width, (float)Height);
        var paint = new SKPaint {
            Color = Color,
        };
        canvas.DrawRect(rect, paint);
        _child.Draw(canvas);
    }
}


public class TextRenderObject: RenderObject {
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
        Font().MeasureText(Value, out var bounds,Paint());
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

    
    public override void SolveMaxConstraints() {}
    
    
    public override void Draw(SKCanvas canvas) {
        var paint = Paint();
        var font = Font();
        // var position = Position;
        // var y = Position.Y + Size.Y;
        
        // Draw at the baseline
        var y = 0 + Height;
        canvas.DrawText(Value, 0, (float)y, font, paint);
    }
}


