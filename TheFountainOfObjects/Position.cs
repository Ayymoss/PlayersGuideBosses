using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Position(byte x = 0, byte y = 0)
{
    public byte X { get; set; } = x;
    public byte Y { get; set; } = y;
    public required RoomBase CurrentRoom { get; set; }
}
