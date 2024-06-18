using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class RoomInstruction
{
    public required RoomBase Room { get; set; }
    public string? Dialogue { get; set; }
    public Choice? PlayerMovement { get; set; }
    public Choice? RoomMovement { get; set; }
    public RoomInstruction? Next { get; set; }
}
