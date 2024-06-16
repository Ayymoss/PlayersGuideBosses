using Humanizer;
using Spectre.Console;
using TicTacToe.Enums;

namespace TicTacToe;

public class Game
{
    private readonly Choice[,] _board = new Choice[3, 3];

    public void StartGame()
    {
        var currentPlayer = Choice.X;
        Choice? winner;

        while (true)
        {
            DisplayBoard();
            Console.WriteLine("--------------------------");

            winner = CalculateWinner();
            if (winner is not Choice.Empty) break;

            HandleUserInput(currentPlayer);
            currentPlayer = currentPlayer is Choice.X ? Choice.O : Choice.X;
        }

        AnsiConsole.MarkupLine(winner is null ? "[yellow]It's a draw![/]" : $"[yellow]{winner} wins![/]");
        AnsiConsole.MarkupLine("[bold yellow]Game Over![/]");
    }

    private void HandleUserInput(Choice player)
    {
        while (true)
        {
            var cell = AnsiConsole.Prompt(new TextPrompt<int>($"[blue]{player.Humanize()}[/] Enter the cell number:")
                .Validate(choice => choice is < 1 or > 9
                    ? ValidationResult.Error("[red]Invalid cell number. Please enter a number between 1 and 9.[/]")
                    : ValidationResult.Success()));

            var row = (cell - 1) / 3;
            var column = (cell - 1) % 3;

            if (_board[row, column] is not Choice.Empty)
            {
                AnsiConsole.MarkupLine("[red]Cell is already taken. Please choose another cell.[/]");
                continue;
            }

            _board[row, column] = player;
            return;
        }
    }

    private void DisplayBoard()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var cell = _board[i, j] is Choice.Empty ? "-" : _board[i, j].ToString();
                Console.Write(cell + " ");
            }

            Console.WriteLine();
        }
    }

    private Choice? CalculateWinner()
    {
        // Check rows [-]
        for (var i = 0; i < 3; i++)
        {
            if (_board[i, 0] == _board[i, 1] && _board[i, 1] == _board[i, 2]) return _board[i, 0];
        }

        // Check columns [|]
        for (var i = 0; i < 3; i++)
        {
            if (_board[0, i] == _board[1, i] && _board[1, i] == _board[2, i]) return _board[0, i];
        }

        // Check diagonal [\]
        if (_board[0, 0] == _board[1, 1] && _board[1, 1] == _board[2, 2]) return _board[0, 0];

        // Check diagonal [/]
        if (_board[0, 2] == _board[1, 1] && _board[1, 1] == _board[2, 0]) return _board[0, 2];

        // Check for draw
        if (_board.Cast<Choice>().All(cell => cell is not Choice.Empty)) return null;

        return Choice.Empty;
    }
}
