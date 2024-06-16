namespace ThePoint;

public static class Program
{
    public static void Main()
    {
        var pointOne = new Point(2, 3);
        var pointTwo = new Point(-4, 0);

        Console.WriteLine($"Point One: (X: {pointOne.X}, Y: {pointOne.Y})");
        Console.WriteLine($"Point Tne: (X: {pointTwo.X}, Y: {pointTwo.Y})");

        // Justification:
        // Currently there is no setter; immutable. This wouldn't work for a lot of scenarios.
        // Typically, we would want to be able to change the values of the properties after initialization, such as a player's position in a game.
        // For this example above we're just setting and returning, so there's no mutability needed.
    }
}
