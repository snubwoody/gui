using System.Numerics;

namespace Agape.Tests;

public class SingleChildLayoutTests {
    [Fact]
    public void AlignMainAxisStart() {
        var rect = new EmptyRenderObject();

        var widget = new SingleChildRenderObject(rect) {
            Constraints = new BoxConstraints(minHeight: 100, minWidth: 200),
            Padding = new Padding(right:20,top:24,bottom:5),
            MainAxisAlignment = AxisAlignment.Start,
            Position = new Vector2(200,0)
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
}