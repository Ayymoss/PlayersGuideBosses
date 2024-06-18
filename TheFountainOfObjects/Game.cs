using Humanizer;
using Spectre.Console;
using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Game
{
    private readonly Player _player;
    private readonly RoomBase[,] _rooms;
    private readonly GameState _gameState = new();

    public Game(GameSize size)
    {
        var (gridSize, maelstromCount) = size switch
        {
            GameSize.Small => (4, 1),
            GameSize.Medium => (5, 2),
            GameSize.Large => (6, 3),
            _ => (5, 2)
        };

        _rooms = new RoomBase[gridSize, gridSize];

        for (byte i = 0; i < gridSize; i++)
        {
            for (byte j = 0; j < gridSize; j++)
            {
                _rooms[i, j] = new Empty { GameState = _gameState };
            }
        }

        _rooms[0, 0] = new Entrance { GameState = _gameState };

        var fountainLocation = GetFreeRoomLocation();
        _rooms[fountainLocation.X, fountainLocation.Y] = new Fountain { GameState = _gameState };

        var pitLocation = GetFreeRoomLocation();
        _rooms[pitLocation.X, pitLocation.Y] = new Pit { GameState = _gameState };

        for (var i = 0; i < maelstromCount; i++)
        {
            var maelstromLocation = GetFreeRoomLocation();
            _rooms[maelstromLocation.X, maelstromLocation.Y] = new Maelstrom { GameState = _gameState };
        }

        _player = new Player { CurrentRoom = _rooms[0, 0] };
    }

    private (byte X, byte Y) GetFreeRoomLocation()
    {
        int x;
        int y;

        do
        {
            (x, y) = (Random.Shared.Next(_rooms.GetUpperBound(0)), Random.Shared.Next(_rooms.GetUpperBound(1)));
        } while (_rooms[x, y] is not Empty);

        return ((byte)x, (byte)y);
    }

    public void StartGame()
    {
        while (!_gameState.IsGameOver)
        {
            PrintPosition();
            var move = HandleUserInput();
            var instructions = HandleChoice(move).EnterRoom();

            do
            {
                if (instructions.Dialogue is not null)
                {
                    HandleDialogue(instructions.Dialogue);
                }

                if (instructions.PlayerMovement is not null)
                {
                    HandleChoice(instructions.PlayerMovement.Value);
                }

                if (instructions.RoomMovement is not null)
                {
                    MoveRoomLocation(instructions.Room, instructions.RoomMovement.Value);
                }

                if (_gameState.IsGameOver) break;
            } while ((instructions = instructions.Next) is not null);
        }
    }

    private static void HandleDialogue(params string[] dialogue)
    {
        foreach (var line in dialogue) AnsiConsole.MarkupLine(line);
    }

    private void PrintPosition()
    {
        HandleDialogue([
            "-------------------------------------------------------------------",
            $"You are in a [blue]{_player.CurrentRoom.GetType().Name}[/] room at [yellow]{_player.X}[/],[yellow]{_player.Y}[/]"
        ]);
    }

    private Choice HandleUserInput()
    {
        List<string> choices =
        [
            "Move " + Choice.East.Humanize(),
            "Move " + Choice.West.Humanize(),
            "Move " + Choice.North.Humanize(),
            "Move " + Choice.South.Humanize()
        ];

        var roomActions = _player.CurrentRoom.GetRoomActions();
        choices.AddRange(roomActions.Select(action => action.Humanize().Titleize()));

        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want to do?")
            .AddChoices(choices));
        return action.Replace("Move ", string.Empty).DehumanizeTo<Choice>();
    }

    private RoomBase HandleChoice(Choice choice)
    {
        switch (choice)
        {
            case Choice.North when _player.Y > _rooms.GetLowerBound(1):
                _player.Y--;
                break;
            case Choice.East when _player.X < _rooms.GetUpperBound(0):
                _player.X++;
                break;
            case Choice.South when _player.Y < _rooms.GetUpperBound(1):
                _player.Y++;
                break;
            case Choice.West when _player.X > _rooms.GetLowerBound(0):
                _player.X--;
                break;
            case Choice.Activate: // Fall through as to not hit the default.
                break;
            default:
                AnsiConsole.MarkupLine("[red]You moved into a wall. Ouch![/]");
                break;
        }

        HandleDialogue(_player.CurrentRoom.HandleRoomAction(choice));
        _player.CurrentRoom = _rooms[_player.X, _player.Y];
        return _player.CurrentRoom;
    }

    private void MoveRoomLocation(RoomBase room, Choice movement)
    {
        var roomLocation = FindRoom(room);

        if (!roomLocation.X.HasValue || !roomLocation.Y.HasValue) return;

        var x = roomLocation.X.Value;
        var y = roomLocation.Y.Value;

        switch (movement)
        {
            case Choice.North when roomLocation.Y > _rooms.GetLowerBound(1):
                if (_rooms[x, y - 1] is Empty)
                {
                    _rooms[x, y] = new Empty { GameState = _gameState };
                    _rooms[x, y - 1] = room;
                }

                break;
            case Choice.East when roomLocation.X < _rooms.GetUpperBound(0):
                if (_rooms[x + 1, y] is Empty)
                {
                    _rooms[x, y] = new Empty { GameState = _gameState };
                    _rooms[x + 1, y] = room;
                }

                break;
            case Choice.South when roomLocation.Y < _rooms.GetUpperBound(1):
                if (_rooms[x, y + 1] is Empty)
                {
                    _rooms[x, y] = new Empty { GameState = _gameState };
                    _rooms[x, y + 1] = room;
                }

                break;
            case Choice.West when roomLocation.X > _rooms.GetLowerBound(0):
                if (_rooms[x - 1, y] is Empty)
                {
                    _rooms[x, y] = new Empty { GameState = _gameState };
                    _rooms[x - 1, y] = room;
                }

                break;
        }
    }

    private (byte? X, byte? Y) FindRoom(RoomBase room)
    {
        for (byte i = 0; i < _rooms.GetUpperBound(0); i++)
        {
            for (byte j = 0; j < _rooms.GetUpperBound(1); j++)
            {
                if (_rooms[i, j] == room) return (i, j);
            }
        }

        return (null, null);
    }
}
