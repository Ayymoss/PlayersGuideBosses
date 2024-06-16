namespace TheColor;

public abstract class ColorFactory
{
    public static Color Create(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Red => new Color(255, 0, 0),
            ColorType.Green => new Color(0, 255, 0),
            ColorType.Blue => new Color(0, 0, 255),
            ColorType.White => new Color(255, 255, 255),
            ColorType.Black => new Color(),
            ColorType.Orange => new Color(255, 165, 0),
            ColorType.Yellow => new Color(255, 255, 0),
            ColorType.Purple => new Color(128, 0, 128),
            _ => new Color()
        };
    }
}
