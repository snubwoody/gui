using SkiaSharp;

namespace Agape;


public abstract class Widget {
    public BoxSizing IntrinsicWidth { get; init; } = new BoxSizing.Shrink();
    public BoxSizing IntrinsicHeight { get; init; } = new BoxSizing.Shrink();
    public BoxConstraints Constraints { get; init; } = new();

    public abstract RenderObject CreateRenderObject();
}

public class Container : Widget {
    public AxisAlignment MainAxisAlignment { get; set; }
    public AxisAlignment CrossAxisAlignment { get; set; }
    public Color Color { get; set; }

    Widget _child;

    public Container(Widget child) {
        _child = child;
    }

    public override RenderObject CreateRenderObject() {
        return new SingleChildRenderObject(child: _child.CreateRenderObject()) {
            Color = Color,
            IntrinsicWidth = IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints,
            MainAxisAlignment = MainAxisAlignment,
            CrossAxisAlignment = CrossAxisAlignment
        };
    }
}

public class Rect : Widget {
    public Color Color { get; set; }

    public override RenderObject CreateRenderObject() {
        return new EmptyRenderObject {
            Color = Color,
            IntrinsicWidth = IntrinsicWidth,
            IntrinsicHeight = IntrinsicHeight,
            Constraints = Constraints
        };
    }
}

public abstract class CompositeWidget : Widget {
    protected abstract Widget Build();

    public override RenderObject CreateRenderObject() {
        return new SingleChildRenderObject(Build().CreateRenderObject());
    }
}
