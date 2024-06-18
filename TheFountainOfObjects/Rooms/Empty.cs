namespace TheFountainOfObjects.Rooms;

public class Empty : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            Dialogue = "[grey]This room is empty.[/]"
        };
    }
}
