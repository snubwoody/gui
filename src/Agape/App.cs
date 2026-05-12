using System.Drawing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SkiaSharp;

namespace Agape;

class App
{
    private readonly IWindow _window;
    private static GL _gl;
    private GRContext _grContext;
    private SKSurface _surface;

    /// <summary>
    /// Instantiate a new app.
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

        var glInterface = GRGlInterface.Create(name =>
            _window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0
        );
        
        _grContext = GRContext.CreateGl(glInterface);

        var framebufferInfo = new GRGlFramebufferInfo(0,0x8058);
        var target = new GRBackendRenderTarget(100, 100,0,8,framebufferInfo);
        
        _surface = SKSurface.Create(_grContext,target,GRSurfaceOrigin.BottomLeft,SKColorType.Rgba8888);
    }

    private void OnRender(double delta)
    {
        var canvas = _surface.Canvas;
        canvas.Clear(SKColors.White);
        
        _grContext.Flush();
    }

    public void Run()
    {
        _window.Run();
    }
}