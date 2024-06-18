using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Rooms;

public class Pit : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            Dialogue = "[red]You have fallen into a pit. You are dead.[/]",
        };
    }
}
