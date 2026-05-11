using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Agape;

class Widget
{
    
}

static class Program
{

	private static IWindow _window;

    static void Main()
    {
		
		var windowOptions = WindowOptions.Default with
		{
			Title = "Agape",
			Size = new Vector2D<int>(500,500),	
			API = new GraphicsAPI(ContextAPI.OpenGL,new APIVersion(3,3))
		};
		_window = Window.Create(windowOptions);
		_window.Run();
        Console.WriteLine("Hello, World!");
    }
}