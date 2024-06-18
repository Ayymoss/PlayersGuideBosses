using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Rooms;

public class Maelstrom : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            PlayerMovement = Choice.MoveNorth,
            RoomMovement = Choice.MoveSouth,
            Next = new RoomInstruction
            {
                Room = this,
                PlayerMovement = Choice.MoveEast,
                RoomMovement = Choice.MoveWest,
                Next = new RoomInstruction
                {
                    Room = this,
                    Dialogue = "[red]You have been sucked into the maelstrom and blown away![/]",
                    PlayerMovement = Choice.MoveEast,
                    RoomMovement = Choice.MoveWest
                }
            }
        };
    }
    
    public override string[] AdjacentRoomCheck() => ["[yellow]You can hear the growling and groaning of a maelstrom nearby.[/]"];
}
