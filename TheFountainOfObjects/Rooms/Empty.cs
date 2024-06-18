using TheFountainOfObjects.Enums;

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

    public override string[] HandleRoomAction(Choice choice)
    {
        return choice is Choice.Attack
            ? ( ["[darkgoldenrod]Your arrow flies through the air and hits the wall. The room is still empty but now contains your arrow.[/]"])
            : base.HandleRoomAction(choice);
    }
}
