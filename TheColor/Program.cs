namespace TheColor;

public static class Program
{
    public static void Main()
    {
        // I felt factory approach was more appropriate for returning new instances of Color.

        var black = ColorFactory.Create(ColorType.Black);
        var red = ColorFactory.Create(ColorType.Red);
        var white = ColorFactory.Create(ColorType.White);
        var greenish = new Color(green: 162);
        var custom = new Color(223, 127, 39);

        Console.WriteLine($"Black: (R: {black.Red}, G: {black.Green}, B: {black.Blue})");
        Console.WriteLine($"Red: (R: {red.Red}, G: {red.Green}, B: {red.Blue})");
        Console.WriteLine($"White: (R: {white.Red}, G: {white.Green}, B: {white.Blue})");
        Console.WriteLine($"Greenish: (R: {greenish.Red}, G: {greenish.Green}, B: {greenish.Blue})");
        Console.WriteLine($"Custom: (R: {custom.Red}, G: {custom.Green}, B: {custom.Blue}");
    }
}
