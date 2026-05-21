using Agape;
namespace Folio;

class Home : CompositeWidget {
    protected override Widget Build() {
        var text = new Text("Hello World") {
            Color = Colors.TextBody
        };
        return new Container(text);
    }
}

static class Program {
    static void Main() {
        var home = new Home();
        var app = new App(home);
        app.Run();
    }
}
