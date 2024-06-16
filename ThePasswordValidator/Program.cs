using Spectre.Console;
using ValidationResult = ThePasswordValidator.Enums.ValidationResult;

namespace ThePasswordValidator;

public static class Program
{
    public static void Main()
    {
        while (true)
        {
            var results = AnsiConsole.Ask<string>("[blue]Test password:[/]").ValidatePassword();

            foreach (var result in results)
            {
                switch (result)
                {
                    case ValidationResult.Ok:
                        AnsiConsole.MarkupLine("[green]Password is valid[/]");
                        break;
                    case ValidationResult.InvalidLength:
                        AnsiConsole.MarkupLine("[red]Password is invalid: invalid length[/]");
                        break;
                    case ValidationResult.ForbiddenCharacters:
                        AnsiConsole.MarkupLine("[red]Password is invalid: forbidden characters ('T', '&')[/]");
                        break;
                    case ValidationResult.NoUpperCase:
                        AnsiConsole.MarkupLine("[red]Password is invalid: no uppercase characters[/]");
                        break;
                    case ValidationResult.NoLowerCase:
                        AnsiConsole.MarkupLine("[red]Password is invalid: no lowercase characters[/]");
                        break;
                    case ValidationResult.NoDigit:
                        AnsiConsole.MarkupLine("[red]Password is invalid: no digits[/]");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(results), results, "Invalid validation result.");
                }
            }
        }
    }
}
