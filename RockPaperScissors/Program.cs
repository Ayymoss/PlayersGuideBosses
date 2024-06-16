using Spectre.Console;

namespace TheChamberOfDesign;

public static class Program
{
    public static void Main()
    {
        // Probably would make more sense in the Game under some setup method, but also gives access to player objects here.
        var playerOneName = AnsiConsole.Ask<string>("[blue]Enter Player[/] [green]One's[/] [blue]name:[/]");
        var playerTwoName = AnsiConsole.Ask<string>("[blue]Enter Player[/] [red]Two's[/] [blue]name:[/]");
        var playerOne = new Player(playerOneName);
        var playerTwo = new Player(playerTwoName);
        var game = new Game(playerOne, playerTwo);

        while (true)
        {
            game.PlayRound();
        }
    }
}
