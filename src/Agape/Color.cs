using SkiaSharp;

namespace Agape;

public readonly record struct Color : IEquatable<Color> {
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    public Color(byte red = 0, byte green = 0, byte blue = 0, byte alpha = 0) {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public Color(byte red = 0, byte green = 0, byte blue = 0) {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = 255;
    }

    public SKColor ToSKColor() {
        return new SKColor(red: Red, green: Green, blue: Blue, alpha: Alpha);
    }

    /// <summary>
    /// Create a color from a hexadecimal code.
    /// </summary>
    public static Color FromHex(string hex) {
        if (!hex.StartsWith('#')) {
            throw new Exception("Missing # at start of hex code");
        }

        switch (hex.Length) {
            case 7:
                var red = Convert.ToByte(hex[1..3], 16);
                var green = Convert.ToByte(hex[3..5], 16);
                var blue = Convert.ToByte(hex[5..7], 16);
                return new(red, green, blue);
            case 9:
                return new(
                    Convert.ToByte(hex[1..3], 16),
                    Convert.ToByte(hex[3..5], 16),
                    Convert.ToByte(hex[5..7], 16),
                    Convert.ToByte(hex[7..9], 16)
                );
            default:
                throw new Exception("Invalid input");
        }

    }
}
