using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class RoomFactory(GameState gameState)
{
    public RoomBase CreateRoom(RoomType roomType) => roomType switch
    {
        RoomType.Amarok => new Amarok { GameState = gameState },
        RoomType.Empty => new Empty { GameState = gameState },
        RoomType.Entrance => new Entrance { GameState = gameState },
        RoomType.Fountain => new Fountain { GameState = gameState },
        RoomType.Maelstrom => new Maelstrom { GameState = gameState },
        RoomType.Pit => new Pit { GameState = gameState },
    };
}
