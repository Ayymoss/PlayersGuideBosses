using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Player(byte x = 0, byte y = 0) : Entity(x, y)
{
    public required RoomBase CurrentRoom { get; set; }
    public int Arrows { get; private set; } = 5;

    public string ShootArrow()
    {
        if (Arrows is 0) return "[darkorange]You have no arrows.[/]";
        Arrows--;
        return "[red1]You shoot an arrow.[/]";
    }
}
