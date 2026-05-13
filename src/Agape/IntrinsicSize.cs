using System.Runtime.InteropServices;

namespace Agape;

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct Padding {
    public float Left { get; set; } = 0;
    public float Right { get; set; } = 0;
    public float Top { get; set; } = 0;
    public float Bottom { get; set; } = 0;

    public Padding(float left = 0, float right = 0, float top = 0, float bottom = 0) {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    public static Padding Zero => All(0);
    public static Padding All(float value) => new(value, value, value, value);
    public static Padding Symmetric(float horizontal, float vertical) => new(horizontal, horizontal, vertical, vertical);
}

public record EmptyLayout : Layout {
    public override List<LayoutDesc> LayoutTree() {
        return [
            new LayoutDesc {
                Id = Id,
                Kind = LayoutKind.Empty,
                IntrinsicWidth = IntrinsicSize.IntrinsicWidth,
                IntrinsicHeight = IntrinsicSize.IntrinsicHeight
            }
        ];
    }
}

public abstract record BoxSizing {
    private BoxSizing() { }

    public sealed record Fixed(float Value) : BoxSizing;
    public sealed record Shrink : BoxSizing;
    public sealed record Flex(uint Factor) : BoxSizing;
}

/// <summary>
/// This is the preferred size of a <see cref="Layout"/> node.
/// </summary>
public readonly record struct IntrinsicSize {
    public BoxSizing Width { get; } = new BoxSizing.Shrink();
    public BoxSizing Height { get; } = new BoxSizing.Shrink();

    public IntrinsicSize(BoxSizing width, BoxSizing height) {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Creates an <see cref="IntrinsicSize"/> with a fixed width and height.
    /// </summary>
    /// <example>
    /// <code>
    /// var intrinsicSize = IntrinsicSize.Fixed(100.0f, 50.0f);
    ///
    /// Debug.Assert(intrinsicSize.Width is BoxSizing.Fixed { Value: 100.0f });
    /// Debug.Assert(intrinsicSize.Height is BoxSizing.Fixed { Value: 50.0f });
    /// </code>
    /// </example>
    public static IntrinsicSize Fixed(float width, float height) {
        return new IntrinsicSize(new BoxSizing.Fixed(width), new BoxSizing.Fixed(height));
    }

    /// <summary>
    /// Creates a new intrinsic size that will be as large as possible.
    /// </summary>
    public static IntrinsicSize Fill() {
        return new IntrinsicSize(new BoxSizing.Flex(1), new BoxSizing.Flex(1));
    }

    public IntrinsicValue IntrinsicWidth => AsIntrinsicValue(Width);
    public IntrinsicValue IntrinsicHeight => AsIntrinsicValue(Height);


    // TODO: maybe move to BoxSizing?
    /// <summary>
    /// Converts the <see cref="BoxSizing"/> into an
    /// <see cref="IntrinsicValue"/>.
    /// </summary>
    private static IntrinsicValue AsIntrinsicValue(BoxSizing size) {
        return size switch {
            BoxSizing.Fixed f => new IntrinsicValue {
                kind = BoxSizingKind.Fixed,
                value = f.Value
            },
            BoxSizing.Flex f => new IntrinsicValue {
                kind = BoxSizingKind.Flex,
                value = f.Factor
            },
            _ => new IntrinsicValue {
                kind = BoxSizingKind.Shrink,
                value = 0
            }
        };
    }
}