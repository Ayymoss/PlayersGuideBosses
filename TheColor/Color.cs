namespace TheColor;

public class Color
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }

    public Color(byte red = 0, byte green = 0, byte blue = 0)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public Color(ColorType color)
    {
        switch (color)
        {
            case ColorType.Red:
                Red = 255;
                break;
            case ColorType.Green:
                Green = 255;
                break;
            case ColorType.Blue:
                Blue = 255;
                break;
            case ColorType.White:
                Red = 255;
                Green = 255;
                Blue = 255;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, "Invalid color type.");
        }
    }
}
