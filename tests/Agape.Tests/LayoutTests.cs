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
}
