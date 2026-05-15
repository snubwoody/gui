using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SkiaSharp;

namespace Agape;

class App {
    private readonly IWindow _window;
    private static GL _gl;
    private GRContext _grContext;
    private SKSurface _surface;
    private Widget widget;
    private RenderObject renderObject;

    /// <summary>
    /// Instantiate a new app.
    /// </summary>
    public App(Widget widget) {
        this.widget = widget;
        renderObject = widget.CreateRenderObject();
        var windowOptions = WindowOptions.Default with {
            Title = "Agape",
            Size = new Vector2D<int>(500, 500),
            API = new GraphicsAPI(ContextAPI.OpenGL, new APIVersion(3, 3))
        };
        _window = Window.Create(windowOptions);
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Resize += OnResize;
    }

    /// <summary>
    /// Create a skia surface.
    /// </summary>
    /// <param name="width">The width of the surface.</param>
    /// <param name="height">The height of the surface.</param>
    /// <returns></returns>
    private SKSurface CreateSurface(int width, int height) {
        // Get the frame buffer address
        _gl.GetInteger(GLEnum.FramebufferBinding, out var framebuffer);

        var framebufferInfo = new GRGlFramebufferInfo((uint)framebuffer, SKColorType.Rgba8888.ToGlSizedFormat());
        var target = new GRBackendRenderTarget(width, height, 0, 8, framebufferInfo);

        return SKSurface.Create(_grContext, target, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
    }

    private void OnLoad() {
        _gl = _window.CreateOpenGL();

        var width = _window.Size.X;
        var height = _window.Size.Y;
        var glInterface = GRGlInterface.Create(name =>
            _gl.Context.TryGetProcAddress(name, out var addr)
                ? addr :
                IntPtr.Zero // Return null pointer for missing functions
        );

        _grContext = GRContext.CreateGl(glInterface);

        _surface = CreateSurface(width, height);
        SolveLayout();
    }

    private void OnResize(Vector2D<int> size) {
        _surface = CreateSurface(size.X, size.Y);
        SolveLayout();
    }

    private void SolveLayout() {
        LayoutSolver.SolveLayout(renderObject, _window.Size.X, _window.Size.Y);
    }

    private void OnRender(double delta) {
        var canvas = _surface.Canvas;
        canvas.Clear(SKColors.White);

        renderObject.Draw(canvas);

        _grContext.Flush();
    }

    public void Run() {
        _window.Run();
    }
}