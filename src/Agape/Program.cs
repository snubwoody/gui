using SkiaSharp;

namespace Agape;

static class Program {
    static void Main() {
        var widget = new Rect {
            IntrinsicWidth = new BoxSizing.Fixed(50),
            IntrinsicHeight = new BoxSizing.Fixed(50),
            Color = SKColors.Blue
        };

        var text = new Text("Hello World");

        var root = new Container(text);

        var app = new App(root);
        app.Run();
    }
}