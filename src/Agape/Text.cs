namespace Agape;

public class Text: Widget {
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
            Value = Value,
            Size = Size,
            IntrinsicWidth =  IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints,
        };
    }
}

