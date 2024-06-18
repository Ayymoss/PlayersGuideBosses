using Humanizer;
using Spectre.Console;
using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects;

public static class Program
{
    public static void Main()
    {
        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What size map do you want?")
            .AddChoices([
                GameSize.Small.Humanize().Titleize(),
                GameSize.Medium.Humanize().Titleize(),
                GameSize.Large.Humanize().Titleize()
            ]));
        var size = action.DehumanizeTo<GameSize>();

        var game = new Game(size);
        game.StartGame();
    }
}
