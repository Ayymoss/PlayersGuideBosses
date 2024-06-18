using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Player(byte x = 0, byte y = 0) : Entity(x, y)
{
    public required RoomBase CurrentRoom { get; set; }
}
