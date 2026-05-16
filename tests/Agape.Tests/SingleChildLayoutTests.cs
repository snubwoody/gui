using System.Numerics;

namespace Agape.Tests;

public class SingleChildLayoutTests {
    [Fact]
    public void AlignMainAxisStart() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            Padding = new Padding(right: 20, top: 24, bottom: 5),
            MainAxisAlignment = AxisAlignment.Start,
            Position = new Vector2(200, 0)
        };

        widget.PositionChildren();
        Assert.Equal(widget.Position.X, widget.Child.Position.X);

        widget.Padding = widget.Padding with {
            Left = 20
        };
        widget.PositionChildren();

        Assert.Equal(widget.Position.X + 20, widget.Child.Position.X);
    }

    [Fact]
    public void AlignMainAxisEnd() {
        var rect = new EmptyRenderObject {
            Width = 20
        };

        var widget = new SingleChildRenderObject(rect) {
            Padding = new Padding(right: 20),
            MainAxisAlignment = AxisAlignment.End,
            Position = new Vector2(200, 0),
            Width = 200
        };

        widget.PositionChildren();
        Assert.Equal(widget.Position.X + 200 - 20, widget.Child.Position.X);
    }

    [Fact]
    public void MinWidthAndHeight() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minHeight: 100, minWidth: 200),
        };

        widget.SolveMinConstraints();
        Assert.Equal(200, widget.Constraints.MinimumWidth);
        Assert.Equal(100, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void MinWidthSmallerThanContentWidth() {
        var rect = new EmptyRenderObject {
            IntrinsicWidth = new BoxSizing.Fixed(500),
        };

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minWidth: 200),
        };

        widget.SolveMinConstraints();
        Assert.Equal(500, widget.Constraints.MinimumWidth);
    }

    [Fact]
    public void MinWidthBiggerThanContentWidth() {
        var rect = new EmptyRenderObject {
            IntrinsicWidth = new BoxSizing.Fixed(500),
        };

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minWidth: 800),
        };

        widget.SolveMinConstraints();
        Assert.Equal(800, widget.Constraints.MinimumWidth);
    }

    [Fact]
    public void MinHeightSmallerThanContentHeight() {
        var rect = new EmptyRenderObject {
            IntrinsicHeight = new BoxSizing.Fixed(500),
        };

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minHeight: 200),
        };

        widget.SolveMinConstraints();
        Assert.Equal(500, widget.Constraints.MinimumHeight);
    }


    [Fact]
    public void MinHeightBiggerThanContentHeight() {
        var rect = new EmptyRenderObject {
            IntrinsicHeight = new BoxSizing.Fixed(500),
        };

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minHeight: 800),
        };

        widget.SolveMinConstraints();
        Assert.Equal(800, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void FixedMinConstraints() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            IntrinsicWidth = new BoxSizing.Fixed(200),
            IntrinsicHeight = new BoxSizing.Fixed(150),
        };

        widget.SolveMinConstraints();
        Assert.Equal(200, widget.Constraints.MinimumWidth);
        Assert.Equal(150, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void ShrinkMinConstraints() {
        var rect = new EmptyRenderObject {
            IntrinsicWidth = new BoxSizing.Fixed(20.5),
            IntrinsicHeight = new BoxSizing.Fixed(10),
        };

        var widget = new SingleChildRenderObject(rect) {
            IntrinsicWidth = new BoxSizing.Shrink(),
            IntrinsicHeight = new BoxSizing.Shrink()
        };

        widget.SolveMinConstraints();
        Assert.Equal(20.5, widget.Constraints.MinimumWidth);
        Assert.Equal(10, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void FlexMaxConstraints() {
        var rect = new EmptyRenderObject {
            IntrinsicWidth = new BoxSizing.Fill(1),
            IntrinsicHeight = new BoxSizing.Fill(1)
        };

        var widget = new SingleChildRenderObject(rect);

        LayoutSolver.SolveLayout(widget, 100, 200);

        Assert.Equal(100, widget.Child.Constraints.MaximumWidth);
        Assert.Equal(200, widget.Child.Constraints.MaximumHeight);
    }

    [Fact]
    public void FlexMaxConstraintsWithPadding() {
        var rect = new EmptyRenderObject {
            IntrinsicWidth = new BoxSizing.Fill(1),
            IntrinsicHeight = new BoxSizing.Fill(1)
        };

        var widget = new SingleChildRenderObject(rect) {
            Padding = new Padding(10, 15, 20, 25)
        };

        LayoutSolver.SolveLayout(widget, 100, 200);

        Assert.Equal(100 - 25, widget.Child.Constraints.MaximumWidth);
        Assert.Equal(200 - 45, widget.Child.Constraints.MaximumHeight);
    }

    [Fact]
    public void AlignCrossAxisStart() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            Position = new Vector2(0, 50),
        };

        widget.PositionChildren();

        Assert.Equal(50, widget.Child.Position.Y);
    }

    [Fact]
    public void AlignCrossAxisStartWithTopPadding() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            Position = new Vector2(0, 50),
            Padding = new Padding(20, 20, 50, 20)
        };

        widget.PositionChildren();

        Assert.Equal(100, widget.Child.Position.Y);
    }

    [Fact]
    public void AlignCrossAxisEnd() {
        var rect = new EmptyRenderObject {
            Height = 20
        };

        var widget = new SingleChildRenderObject(rect) {
            Position = new Vector2(0, 50),
            Padding = new Padding(20, 20, 50, 40),
            CrossAxisAlignment = AxisAlignment.End
        };

        widget.PositionChildren();

        Assert.Equal(widget.Position.Y - 20 - 40, widget.Child.Position.Y);
    }
}