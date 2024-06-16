namespace TheColor;

public abstract class ColorFactory
{
    public static Color Create(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Black => new Color(),
            ColorType.Red => new Color(255),
            ColorType.Green => new Color(green: 255),
            ColorType.Blue => new Color(blue: 255),
            ColorType.Purple => new Color(128, blue: 128),
            ColorType.Orange => new Color(255, 165),
            ColorType.Yellow => new Color(255, 255),
            ColorType.White => new Color(255, 255, 255),
            _ => new Color()
        };
    }
}
