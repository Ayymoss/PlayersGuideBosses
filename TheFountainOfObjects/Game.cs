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

        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                _rooms[i, j] = new Empty { GameState = _gameState };
            }
        }

        _rooms[0, 0] = new Entrance { GameState = _gameState };

        var fountainLocation = GetEmptyRoomLocation();
        _rooms[fountainLocation.X, fountainLocation.Y] = new Fountain { GameState = _gameState };

        var pitLocation = GetEmptyRoomLocation();
        _rooms[pitLocation.X, pitLocation.Y] = new Pit { GameState = _gameState };

        for (var i = 0; i < maelstromCount; i++)
        {
            var maelstromLocation = GetEmptyRoomLocation();
            _rooms[maelstromLocation.X, maelstromLocation.Y] = new Maelstrom { GameState = _gameState };

            var amarokLocation = GetEmptyRoomLocation();
            _rooms[amarokLocation.X, amarokLocation.Y] = new Amarok { GameState = _gameState };
        }

        _player = new Player { CurrentRoom = _rooms[0, 0] };
    }

    private (byte X, byte Y) GetEmptyRoomLocation()
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
            CheckAdjacentRooms();
            var move = HandleUserInput();
            var instructions = HandleChoice(move).EnterRoom();

            do // TODO: Handle rooms if moved through many quickly (e.g. maelstrom). Currently they're skipped over.
            {
                if (instructions.Dialogue is not null) HandleDialogue(instructions.Dialogue);
                if (instructions.PlayerMovement is not null) HandleChoice(instructions.PlayerMovement.Value);
                if (instructions.RoomMovement is not null) MoveRoomLocation(instructions.Room, instructions.RoomMovement.Value);

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
            $"You are in a [blue]{_player.CurrentRoom.GetType().Name}[/] room at [yellow]{_player.X}[/],[yellow]{_player.Y}[/]. Your quiver holds [lime]{_player.Arrows} arrows[/]."
        ]);
    }

    private Choice HandleUserInput()
    {
        IEnumerable<string> choices =
        [
            Choice.MoveNorth.Humanize().Titleize(),
            Choice.MoveEast.Humanize().Titleize(),
            Choice.MoveSouth.Humanize().Titleize(),
            Choice.MoveWest.Humanize().Titleize(),

            Choice.ShootNorth.Humanize().Titleize(),
            Choice.ShootEast.Humanize().Titleize(),
            Choice.ShootSouth.Humanize().Titleize(),
            Choice.ShootWest.Humanize().Titleize()
        ];

        var roomActions = _player.CurrentRoom.GetRoomActions();
        choices = roomActions.Select(action => action.Humanize().Titleize()).Concat(choices);

        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want to do?")
            .AddChoices(choices));
        return action.DehumanizeTo<Choice>();
    }

    private RoomBase HandleChoice(Choice choice)
    {
        switch (choice)
        {
            case Choice.MoveNorth:
                HandlePlayerMovement(_player.X, _player.Y - 1);
                break;
            case Choice.MoveEast:
                HandlePlayerMovement(_player.X + 1, _player.Y);
                break;
            case Choice.MoveSouth:
                HandlePlayerMovement(_player.X, _player.Y + 1);
                break;
            case Choice.MoveWest:
                HandlePlayerMovement(_player.X - 1, _player.Y);
                break;
            case Choice.ShootNorth:
                HandleArrowFire(_player.X, _player.Y - 1);
                break;
            case Choice.ShootEast:
                HandleArrowFire(_player.X + 1, _player.Y);
                break;
            case Choice.ShootSouth:
                HandleArrowFire(_player.X, _player.Y + 1);
                break;
            case Choice.ShootWest:
                HandleArrowFire(_player.X - 1, _player.Y);
                break;
        }

        HandleDialogue(_player.CurrentRoom.HandleRoomAction(choice));
        _player.CurrentRoom = _rooms[_player.X, _player.Y];
        return _player.CurrentRoom;
    }

    private void HandlePlayerMovement(int x, int y)
    {
        if (!IsValidGamePosition((byte)x, (byte)y))
        {
            HandleDialogue("[darkorange3]You moved into a wall. Ouch![/]");
            return;
        }

        _player.X = (byte)x;
        _player.Y = (byte)y;
    }

    private void HandleArrowFire(int x, int y)
    {
        HandleDialogue(_player.ShootArrow());

        if (!IsValidGamePosition((byte)x, (byte)y))
        {
            HandleDialogue("[darkorange3]You shot into the void.[/]");
            return;
        }

        HandleDialogue(_rooms[x, y].HandleRoomAction(Choice.Attack));
    }

    private void MoveRoomLocation(RoomBase room, Choice movement)
    {
        var roomLocation = FindRoom(room);

        if (!roomLocation.X.HasValue || !roomLocation.Y.HasValue) return;

        var x = roomLocation.X.Value;
        var y = roomLocation.Y.Value;

        switch (movement)
        {
            case Choice.MoveNorth:
                HandleRoomMove(room, x, y, x, y - 1);
                break;
            case Choice.MoveEast:
                HandleRoomMove(room, x, y, x + 1, y);
                break;
            case Choice.MoveSouth:
                HandleRoomMove(room, x, y, x, y + 1);
                break;
            case Choice.MoveWest:
                HandleRoomMove(room, x, y, x - 1, y);
                break;
        }
    }

    private void HandleRoomMove(RoomBase room, int oldX, int oldY, int newX, int newY)
    {
        if (!IsValidGamePosition(newX, newY) || _rooms[newX, newY] is not Empty) (newX, newY) = GetEmptyRoomLocation();

        _rooms[oldX, oldY] = new Empty { GameState = _gameState };
        _rooms[newX, newY] = room;
    }

    private void CheckAdjacentRooms()
    {
        List<string> dialogue = [];
        for (var i = _player.X - 1; i <= _player.X + 1; i++)
        {
            for (var j = _player.Y - 1; j <= _player.Y + 1; j++)
            {
                var isCurrentRoom = i == _player.X && j == _player.Y;
                if (isCurrentRoom || !IsValidGamePosition(i, j)) continue;

                var room = _rooms[i, j];
                dialogue.AddRange(room.AdjacentRoomCheck());
            }
        }

        HandleDialogue(dialogue.Distinct().ToArray());
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

    private bool IsValidGamePosition(int x, int y)
    {
        var crossLeftBoundary = x < _rooms.GetLowerBound(0);
        var crossRightBoundary = x > _rooms.GetUpperBound(0);
        var crossUpperBoundary = y < _rooms.GetLowerBound(1);
        var crossLowerBoundary = y > _rooms.GetUpperBound(1);

        return !(crossLeftBoundary || crossRightBoundary || crossUpperBoundary || crossLowerBoundary);
    }
}
