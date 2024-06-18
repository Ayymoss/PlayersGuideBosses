namespace TheFountainOfObjects.Rooms;

public class Amarok : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            Dialogue = "[red]You are in a room with a large wolf. You died.[/]"
        };
    }

    public override string[] AdjacentRoomCheck() => ["You can smell the rotten stench of an amarok in a nearby room."];

    public override RoomInstruction EnterRoom()
    {
        GameState.IsGameOver = true;
        return base.EnterRoom();
    }
}
