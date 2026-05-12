using System.Drawing;
using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Agape;

class Widget
{
    
}

class App
{
	private readonly IWindow _window;
	private static GL _gl;

	/// <summary>
	/// Instantiates a new app.
	/// </summary>
	public App()
	{
		var windowOptions = WindowOptions.Default with
		{
			Title = "Agape",
			Size = new Vector2D<int>(500,500),	
			API = new GraphicsAPI(ContextAPI.OpenGL,new APIVersion(3,3))
		};
		_window = Window.Create(windowOptions);
		_window.Load += OnLoad;
		_window.Render += OnRender;
	}

	private void OnLoad()
	{
		_gl = _window.CreateOpenGL();
		_gl.ClearColor(Color.White);
	}

	private void OnRender(double delta)
	{
		_gl.Clear(ClearBufferMask.ColorBufferBit);
	}

	public void Run()
	{
		_window.Run();
	}
}

static class Program
{
    static void Main()
    {
		
	    var app  = new App();
	    app.Run();
    }
}