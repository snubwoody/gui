using System.Numerics;
using System.Runtime.InteropServices;

namespace Agape;

/// <summary>
/// A globally unique identifier.
/// </summary>
[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct GlobalId {
    public uint Value { get; } = create_global_id().Value;
    
    public GlobalId() {
        Value = create_global_id().Value;
    }

    [DllImport("cascada", CallingConvention = CallingConvention.Cdecl)]
    static extern GlobalId create_global_id();
}

public abstract record Layout {
    public GlobalId Id { get; set; } = new();
    public IntrinsicSize IntrinsicSize { get; set; }
    public AxisAlignment MainAxisAlignment { get; set; }
    public AxisAlignment CrossAxisAlignment { get; set; }

    public abstract List<LayoutDesc> LayoutTree();
}

public static class LayoutSolver {
    [DllImport("cascada", CallingConvention = CallingConvention.Cdecl)]
    public static extern void solve_layout_from_desc(
        [In] LayoutDesc[] descs,
        UIntPtr len,
        [Out] LayoutNode[] outBuffer,
        Size size
        );

    /// <summary>
    /// Calculates and applies the final size and position of all the
    /// render objects in the tree.
    /// </summary>
    /// <param name="renderObject">The root <see cref="RenderObject"/>.</param>
    /// <param name="windowSize">The size of the window.</param>
    public static void SolveLayout(Vector2 windowSize)
    {
        var layout = new EmptyLayout
        {
            IntrinsicSize = IntrinsicSize.Fill()
        };
        
        var layoutTree = layout.LayoutTree();
        // var layoutTree = view.CreateLayout().LayoutTree();
        var layoutNodes = new LayoutNode[layoutTree.Count];
        var size = new Size {
            Width = windowSize.X,
            Height = windowSize.Y
        };
        solve_layout_from_desc(
            layoutTree.ToArray(),
            (UIntPtr)layoutTree.Count,
            layoutNodes,
            size
        );
        Console.WriteLine(layoutNodes[0]);
        // view.UpdateBounds(layoutNodes);
    }
}

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct LayoutDesc {
    public GlobalId Id = new();
    public LayoutKind Kind = LayoutKind.Empty;
    public IntrinsicValue IntrinsicWidth = default;
    public IntrinsicValue IntrinsicHeight = default;
    public Padding Padding;
    public uint Spacing = 0;
    public AxisAlignment MainAxisAlignment;
    public AxisAlignment CrossAxisAlignment;
    public UIntPtr ChildCount = 0;

    public LayoutDesc() { }
}

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct IntrinsicValue {
    public BoxSizingKind kind;
    public float value;
}

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct Size(float Width, float Height);

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct Position(float X, float Y);

[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public record struct LayoutNode {
    public GlobalId Id;
    public Size Size;
    public Position Position;
}

public enum LayoutKind : byte {
    Empty = 0,
    Block = 1,
    Horizontal = 2,
    Vertical = 3
}

public enum AxisAlignment : byte {
    Start = 0,
    Center = 1,
    End = 2
}

public enum BoxSizingKind : byte {
    Shrink = 0,
    Flex = 1,
    Fixed = 2
}