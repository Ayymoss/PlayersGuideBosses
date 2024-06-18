using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Rooms;

public class Maelstrom : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            Dialogue = "[yellow]You hear the growling and groaning of a maelstrom nearby.[/]",
            PlayerMovement = Choice.North,
            RoomMovement = Choice.South,
            Next = new RoomInstruction
            {
                Room = this,
                PlayerMovement = Choice.East,
                RoomMovement = Choice.West,
                Next = new RoomInstruction
                {
                    Room = this,
                    Dialogue = "[red]You have been sucked into the maelstrom and blown away![/]",
                    PlayerMovement = Choice.East,
                    RoomMovement = Choice.West
                }
            }
        };
    }
}
