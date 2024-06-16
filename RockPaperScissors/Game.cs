using Humanizer;
using Spectre.Console;
using TheChamberOfDesign.Enums;

namespace TheChamberOfDesign;

public class Game(Player playerOne, Player playerTwo)
{
    private record Round(Choice PlayerOne, Choice PlayerTwo);

    private int _roundsPlayed;

    private readonly Dictionary<Round, Winner> _resultMap = new()
    {
        { new Round(Choice.Rock, Choice.Scissors), Winner.PlayerOne },
        { new Round(Choice.Scissors, Choice.Paper), Winner.PlayerOne },
        { new Round(Choice.Paper, Choice.Rock), Winner.PlayerOne },

        { new Round(Choice.Rock, Choice.Paper), Winner.PlayerTwo },
        { new Round(Choice.Scissors, Choice.Rock), Winner.PlayerTwo },
        { new Round(Choice.Paper, Choice.Scissors), Winner.PlayerTwo },

        { new Round(Choice.Rock, Choice.Rock), Winner.Draw },
        { new Round(Choice.Paper, Choice.Paper), Winner.Draw },
        { new Round(Choice.Scissors, Choice.Scissors), Winner.Draw }
    };

    public void PlayRound()
    {
        _roundsPlayed++;

        var playerOneChoice = AskUserChoice($"\n[green]{playerOne.Name}[/], choose your weapon:");
        Console.Clear();
        var playerTwoChoice = AskUserChoice($"[red]{playerTwo.Name}[/], choose your weapon:");
        Console.Clear();

        AnsiConsole.MarkupLine($"[yellow]Round Choices:[/] [green]{playerOneChoice}[/] vs [red]{playerTwoChoice}[/]");

        var round = new Round(playerOneChoice, playerTwoChoice);
        HandleWinner(round);
        PrintStatus();
    }

    private void PrintStatus()
    {
        Console.WriteLine("-----------------------------------------");
        AnsiConsole.MarkupLine($"[yellow]Round: {_roundsPlayed}[/]");
        AnsiConsole.MarkupLine($"[green]{playerOne.Name}: {playerOne.Wins} wins ({playerOne.WinRate:P})[/]");
        AnsiConsole.MarkupLine($"[red]{playerTwo.Name}: {playerTwo.Wins} wins ({playerTwo.WinRate:P})[/]");
    }

    private static Choice AskUserChoice(string message)
    {
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(message)
                .AddChoices([
                    Choice.Paper.Humanize().Titleize(),
                    Choice.Scissors.Humanize().Titleize(),
                    Choice.Rock.Humanize().Titleize()
                ]));
        return action.DehumanizeTo<Choice>();
    }

    private void HandleWinner(Round round)
    {
        var winner = _resultMap[round];

        switch (winner)
        {
            case Winner.PlayerOne:
                playerOne.Wins++;
                playerTwo.Losses++;
                AnsiConsole.MarkupLine($"[green]{playerOne.Name} wins![/]");
                break;
            case Winner.PlayerTwo:
                playerTwo.Wins++;
                playerOne.Losses++;
                AnsiConsole.MarkupLine($"[red]{playerTwo.Name} wins![/]");
                break;
            case Winner.Draw:
                playerOne.Draws++;
                playerTwo.Draws++;
                AnsiConsole.MarkupLine("[blue]It's a draw![/]");
                break;
        }
    }
}
