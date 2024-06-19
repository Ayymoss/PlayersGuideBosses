using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class RoomFactory(GameState gameState)
{
    public Empty CreateEmptyRoom() => new() { GameState = gameState };
    public Fountain CreateFountainRoom() => new() { GameState = gameState };
    public Pit CreatePitRoom() => new() { GameState = gameState };
    public Maelstrom CreateMaelstromRoom() => new() { GameState = gameState };
    public Amarok CreateAmarokRoom() => new() { GameState = gameState };
    public Entrance CreateEntranceRoom() => new() { GameState = gameState };
}
