namespace Agape;

public class Text : Widget {
    /// <summary>
    /// The font color.
    /// </summary>
    public Color Color { get; init; } = new(0, 0, 0);

    public string Value { get; init; }

    /// <summary>
    /// The font size of the text.
    /// </summary>
    public double Size { get; init; } = 16;

    public Text(string value) {
        Value = value;
    }

    public override RenderObject CreateRenderObject() {
        return new TextRenderObject(Value) {
            Color = Color,
            Value = Value,
            Size = Size,
            IntrinsicWidth = IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints,
        };
    }
}

