using SkiaSharp;

namespace Agape;

static class Program {
    static void Main() {
        var widget = new Rect {
            IntrinsicWidth = new BoxSizing.Fixed(50),
            IntrinsicHeight = new BoxSizing.Fixed(50),
            Color = SKColors.Blue
        };

        var app = new App(widget);
        app.Run();
    }
}