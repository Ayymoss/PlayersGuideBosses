using Spectre.Console;

namespace Manticore.Utilities;

public static class Extensions
{
    public static int IntHelper(this string message)
    {
        while (true)
        {
            AnsiConsole.Markup($"{message} ");
            if (int.TryParse(Console.ReadLine(), out var result)) return result;
            AnsiConsole.WriteLine("Invalid input. Please try again.");
        }
    }
}
