using Humanizer;
using Spectre.Console;
using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Game
{
    private Position _position = null!;
    private RoomBase[,] _rooms = null!;
    private bool _gameOver;

    public void SetupGame()
    {
        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What size map do you want?")
            .AddChoices([
                GameSize.Small.Humanize().Titleize(),
                GameSize.Medium.Humanize().Titleize(),
                GameSize.Large.Humanize().Titleize()
            ]));
        var size = action.DehumanizeTo<GameSize>();

        var gridSize = size switch
        {
            GameSize.Small => 4,
            GameSize.Medium => 5,
            GameSize.Large => 6,
            _ => 5
        };

        _rooms = new RoomBase[gridSize, gridSize];

        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                _rooms[i, j] = new Empty();
            }
        }

        var fountain = new Fountain();
        var entrance = new Entrance { Fountain = fountain };
        var pit = new Pit();

        var validPlacement = false;
        while (!validPlacement)
        {
            var (fountainX, fountainY) = (Random.Shared.Next(gridSize), Random.Shared.Next(gridSize));
            var (pitX, pitY) = (Random.Shared.Next(gridSize), Random.Shared.Next(gridSize));

            validPlacement = (fountainX, fountainY) != (0, 0)
                             && (pitX, pitY) != (0, 0)
                             && (fountainX, fountainY) != (pitX, pitY);

            if (!validPlacement) continue;
            _rooms[fountainX, fountainY] = fountain;
            _rooms[pitX, pitY] = pit;
        }

        _rooms[0, 0] = entrance;
        _position = new Position { CurrentRoom = _rooms[0, 0] };

        entrance.IsFountainActivated += () => _gameOver = true;
        pit.HasFallen += () => _gameOver = true;
    }

    public void StartGame()
    {
        while (!_gameOver)
        {
            PrintPosition();
            var move = HandleUserInput();
            HandleMovement(move);
        }
    }

    private void PrintPosition()
    {
        Console.WriteLine("-------------------------------------------------------------------");
        AnsiConsole.MarkupLine(
            $"You are in a [blue]{_position.CurrentRoom.GetType().Name}[/] room at [yellow]{_position.X}[/],[yellow]{_position.Y}[/]");
        AnsiConsole.MarkupLine(_position.CurrentRoom.RoomDialogue());
    }

    private Choice HandleUserInput()
    {
        if (_gameOver) return Choice.East; // Early exit if the game is over - no need to prompt the user and hold the main thread

        List<string> choices =
        [
            "Move " + Choice.East.Humanize(),
            "Move " + Choice.West.Humanize(),
            "Move " + Choice.North.Humanize(),
            "Move " + Choice.South.Humanize()
        ];
        if (_position.CurrentRoom is Fountain { Activated: false }) choices.Add(Choice.Activate.Humanize().Titleize());

        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want to do?")
            .AddChoices(choices));
        return action.Replace("Move ", string.Empty).DehumanizeTo<Choice>();
    }

    private void HandleMovement(Choice move)
    {
        switch (move)
        {
            case Choice.North when _position.Y > _rooms.GetLowerBound(1):
                _position.Y--;
                break;
            case Choice.East when _position.X < _rooms.GetUpperBound(0):
                _position.X++;
                break;
            case Choice.South when _position.Y < _rooms.GetUpperBound(1):
                _position.Y++;
                break;
            case Choice.West when _position.X > _rooms.GetLowerBound(0):
                _position.X--;
                break;
            case Choice.Activate when _position.CurrentRoom is Fountain fountain:
                if (fountain.Activated)
                {
                    AnsiConsole.MarkupLine("[red]You have already activated the Fountain of Objects[/]");
                    break;
                }

                AnsiConsole.MarkupLine("You have activated the Fountain of Objects and the cavern is now collapsing!");
                AnsiConsole.MarkupLine("You must escape to the entrance to survive!");

                fountain.Activated = true;
                break;
            default:
                AnsiConsole.MarkupLine("[red]You've walked into a wall. Ouch![/]");
                break;
        }

        _position.CurrentRoom = _rooms[_position.X, _position.Y];
        _position.CurrentRoom.Visited = true;
    }
}
