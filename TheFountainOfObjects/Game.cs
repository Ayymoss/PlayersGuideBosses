using Humanizer;
using Spectre.Console;
using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Game
{
    private readonly Player _player;
    private readonly RoomBase[,] _rooms;
    private readonly GameState _gameState = new();
    private readonly RoomFactory _roomFactory;

    public Game(GameSize size)
    {
        _roomFactory = new RoomFactory(_gameState);
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
                _rooms[i, j] = _roomFactory.CreateEmptyRoom();
            }
        }

        _rooms[0, 0] = _roomFactory.CreateEntranceRoom();
        _rooms[2, 0] = _roomFactory.CreateMaelstromRoom(); // TODO: Remove this line

        var fountainLocation = GetEmptyRoomLocation();
        _rooms[fountainLocation.X, fountainLocation.Y] = _roomFactory.CreateFountainRoom();

        var pitLocation = GetEmptyRoomLocation();
        _rooms[pitLocation.X, pitLocation.Y] = _roomFactory.CreatePitRoom();

        for (var i = 0; i < maelstromCount; i++)
        {
            var maelstromLocation = GetEmptyRoomLocation();
            _rooms[maelstromLocation.X, maelstromLocation.Y] = _roomFactory.CreateMaelstromRoom();

            var amarokLocation = GetEmptyRoomLocation();
            _rooms[amarokLocation.X, amarokLocation.Y] = _roomFactory.CreateAmarokRoom();
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
        Stack<InstructionBase> instructions = new();
        PrintPosition();
        CheckAdjacentRooms();

        while (!_gameState.IsGameOver)
        {
            var move = HandleUserInput();
            var room = HandleChoice(move);

            var newInstructionRoom = room.EnterRoom();
            PrintPosition();
            CheckAdjacentRooms();
            newInstructionRoom.Reverse();
            foreach (var instruction in newInstructionRoom) instructions.Push(instruction);

            while (instructions.Count != 0)
            {
                if (room != _player.CurrentRoom)
                {
                    room = _player.CurrentRoom;
                    newInstructionRoom = room.EnterRoom();
                    newInstructionRoom.Reverse();
                    foreach (var instruction in newInstructionRoom) instructions.Push(instruction);
                }

                var currentInstruction = instructions.Pop();
                HandleInstruction(currentInstruction);
                if (_gameState.IsGameOver) break;
            }
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
        var choices = _player.GetChoices().Select(x => x.Humanize().Titleize());
        var roomActions = _player.CurrentRoom.GetRoomActions();
        choices = roomActions.Select(x => x.Humanize().Titleize()).Concat(choices);

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

        // TODO: Maybe move the EnterRoom call here so we can recursively handle each room's instructions?
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

    private void MoveRoomLocation(int currentX, int currentY, Choice movement)
    {
        var newX = currentX;
        var newY = currentY;

        switch (movement)
        {
            case Choice.MoveNorth:
                newY--;
                break;
            case Choice.MoveEast:
                newX++;
                break;
            case Choice.MoveSouth:
                newY++;
                break;
            case Choice.MoveWest:
                newX--;
                break;
        }

        if (!IsValidGamePosition(newX, newY) || _rooms[newX, newY] is not Empty) (newX, newY) = GetEmptyRoomLocation();

        _rooms[newX, newY] = _rooms[currentX, currentY];
        _rooms[currentX, currentY] = _roomFactory.CreateEmptyRoom();
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

    private void HandleInstruction(InstructionBase instruction)
    {
        switch (instruction)
        {
            case MovePlayerInstruction movePlayerInstruction:
                PrintPosition();
                CheckAdjacentRooms();
                HandleChoice(movePlayerInstruction.Move);
                break;
            case MoveRoomInstruction moveRoomInstruction:
                MoveRoomLocation(_player.X, _player.Y, moveRoomInstruction.Move);
                break;
            case DialogueInstruction dialogueInstruction:
                HandleDialogue(dialogueInstruction.Dialogue);
                break;
        }
    }
}
