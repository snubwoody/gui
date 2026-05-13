namespace Agape.Tests;

public class LayoutTests
{
    [Fact]
    public void FixedMinConstraints()
    {
        var rect = new Rect
        {
            IntrinsicWidth = new BoxSizing.Fixed(50),
            IntrinsicHeight = new BoxSizing.Fixed(20),
        };

        rect.SolveMinConstraints();
        Assert.Equal(50, rect.Constraints.MinimumWidth);
        Assert.Equal(20, rect.Constraints.MinimumHeight);
    }

    [Fact]
    public void FixedMinConstraintsPrecedence()
    {
        var rect = new Rect
        {
            Constraints = new BoxConstraints(minHeight: 100, minWidth: 200),
            IntrinsicWidth = new BoxSizing.Fixed(50),
            IntrinsicHeight = new BoxSizing.Fixed(20),
        };

        rect.SolveMinConstraints();
        Assert.Equal(200, rect.Constraints.MinimumWidth);
        Assert.Equal(100, rect.Constraints.MinimumHeight);
    }

    [Fact]
    public void EmptyWidgetMinWidthAndHeight()
    {
        var rect = new Rect
        {
            Constraints = new BoxConstraints(minHeight: 100, minWidth: 200),
        };

        rect.SolveMinConstraints();
        Assert.Equal(200, rect.Constraints.MinimumWidth);
        Assert.Equal(100, rect.Constraints.MinimumHeight);
    }

    [Fact]
    public void SingleChildWidgetMinWidthAndHeight()
    {
        var rect = new Rect();

        var widget = new Container(rect)
        {
            Constraints = new BoxConstraints(minHeight: 100, minWidth: 200),
        };

        widget.SolveMinConstraints();
        Assert.Equal(200, widget.Constraints.MinimumWidth);
        Assert.Equal(100, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void SingleChildWidgetFixedMinConstraints()
    {
        var rect = new Rect();

        var widget = new Container(rect)
        {
            IntrinsicWidth = new BoxSizing.Fixed(200),
            IntrinsicHeight = new BoxSizing.Fixed(150),
        };

        widget.SolveMinConstraints();
        Assert.Equal(200, widget.Constraints.MinimumWidth);
        Assert.Equal(150, widget.Constraints.MinimumHeight);
    }

    [Fact]
    public void SingleChildWidgetShrinkMinConstraints()
    {
        var rect = new Rect
        {
            IntrinsicWidth = new BoxSizing.Fixed(20.5),
            IntrinsicHeight = new BoxSizing.Fixed(10),
        };

        var widget = new Container(rect)
        {
            IntrinsicWidth = new BoxSizing.Shrink(),
            IntrinsicHeight = new BoxSizing.Shrink()
        };

        widget.SolveMinConstraints();
        Assert.Equal(20.5, widget.Constraints.MinimumWidth);
        Assert.Equal(10, widget.Constraints.MinimumHeight);
    }
}
