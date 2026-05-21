using Agape;
namespace Folio;

public abstract class Colors {
    public static readonly Color White = new Color(255, 255, 255);
    public static readonly Color Black = new Color(0, 0, 0);

    // Neutral
    public static readonly Color Neutral25 = Color.FromHex("#FCFCFC");
    public static readonly Color Neutral50 = Color.FromHex("#F7F7F7");
    public static readonly Color Neutral100 = Color.FromHex("#E6E6E6");
    public static readonly Color Neutral200 = Color.FromHex("#CBCBCB");
    public static readonly Color Neutral300 = Color.FromHex("#BEBEBE");
    public static readonly Color Neutral400 = Color.FromHex("#9E9E9E");
    public static readonly Color Neutral500 = Color.FromHex("#808080");
    public static readonly Color Neutral600 = Color.FromHex("#787878");
    public static readonly Color Neutral700 = Color.FromHex("#626262");
    public static readonly Color Neutral800 = Color.FromHex("#3E3E3E");
    public static readonly Color Neutral900 = Color.FromHex("#282828");
    public static readonly Color Neutral950 = Color.FromHex("#171717");

    // Purple
    public static readonly Color Purple50 = Color.FromHex("#EEECFD");
    public static readonly Color Purple100 = Color.FromHex("#E0DCFB");
    public static readonly Color Purple200 = Color.FromHex("#C3B9F8");
    public static readonly Color Purple300 = Color.FromHex("#A796F4");
    public static readonly Color Purple400 = Color.FromHex("#896FEF");
    public static readonly Color Purple500 = Color.FromHex("#7048E9");
    public static readonly Color Purple600 = Color.FromHex("#5B28D4");
    public static readonly Color Purple700 = Color.FromHex("#471EA9");
    public static readonly Color Purple800 = Color.FromHex("#31137A");
    public static readonly Color Purple900 = Color.FromHex("#1D084F");
    public static readonly Color Purple950 = Color.FromHex("#110434");

    // Blue
    public static readonly Color Blue50 = Color.FromHex("#E5F2FF");
    public static readonly Color Blue100 = Color.FromHex("#CEE9FF");
    public static readonly Color Blue200 = Color.FromHex("#95D3FF");
    public static readonly Color Blue300 = Color.FromHex("#32BFFF");
    public static readonly Color Blue400 = Color.FromHex("#03A5E0");
    public static readonly Color Blue500 = Color.FromHex("#0088B9");
    public static readonly Color Blue600 = Color.FromHex("#006B92");
    public static readonly Color Blue700 = Color.FromHex("#015170");
    public static readonly Color Blue800 = Color.FromHex("#00374D");
    public static readonly Color Blue900 = Color.FromHex("#002030");
    public static readonly Color Blue950 = Color.FromHex("#00131E");

    // Green
    public static readonly Color Green50 = Color.FromHex("#EBFFE9");
    public static readonly Color Green100 = Color.FromHex("#C8FFC4");
    public static readonly Color Green200 = Color.FromHex("#8EFE80");
    public static readonly Color Green300 = Color.FromHex("#4FF720");
    public static readonly Color Green400 = Color.FromHex("#49E71C");
    public static readonly Color Green500 = Color.FromHex("#44D81A");
    public static readonly Color Green600 = Color.FromHex("#33A913");
    public static readonly Color Green700 = Color.FromHex("#247E0A");
    public static readonly Color Green800 = Color.FromHex("#155504");
    public static readonly Color Green900 = Color.FromHex("#072D01");
    public static readonly Color Green950 = Color.FromHex("#031D01");

    // Red
    public static readonly Color Red50 = Color.FromHex("#FEEDED");
    public static readonly Color Red100 = Color.FromHex("#FEDADA");
    public static readonly Color Red200 = Color.FromHex("#FDB4B4");
    public static readonly Color Red300 = Color.FromHex("#FC8A8B");
    public static readonly Color Red400 = Color.FromHex("#FB5758");
    public static readonly Color Red500 = Color.FromHex("#EC181C");
    public static readonly Color Red600 = Color.FromHex("#BD1013");
    public static readonly Color Red700 = Color.FromHex("#910B0C");
    public static readonly Color Red800 = Color.FromHex("#670406");
    public static readonly Color Red900 = Color.FromHex("#400202");
    public static readonly Color Red950 = Color.FromHex("#2C0101");

    // Border
    public static readonly Color BorderNeutral10 = Colors.Neutral50;
    public static readonly Color BorderNeutralFocus = Colors.Neutral500;

    // Surface
    public static readonly Color SurfacePrimary = Colors.Purple500;
    public static readonly Color SurfacePrimaryHover = Colors.Purple600;
    public static readonly Color SurfacePrimaryActive = Colors.Purple700;

    // Text
    public static readonly Color TextHeading = Neutral950;
    public static readonly Color TextBody = Neutral900;
    public static readonly Color TextMuted = Neutral600;
    public static readonly Color TextError = Red400;
}


