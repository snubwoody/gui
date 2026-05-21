using SkiaSharp;

namespace Agape.Tests;

public class ColorTests {
    [Fact]
    public void ToSKColor() {
        var color = new Color(255, 22, 124, 25);
        var skColor = color.ToSKColor();
        Assert.Equal(skColor, new SKColor(255, 22, 124, 25));
    }

    [Fact]
    public void FromHexMissingHash() {
        Assert.Throws<Exception>(() => {
            var color = Color.FromHex("000000");
        });
    }

    [Fact]
    public void FromHex6() {
        var color = Color.FromHex("#cbf542");
        Assert.Equal(new Color(203, 245, 66), color);
    }

    [Fact]
    public void FromHex8() {
        var color = Color.FromHex("#cbf54200");
        Assert.Equal(new Color(203, 245, 66, 0), color);
    }
}
