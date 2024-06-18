namespace TheFountainOfObjects.Rooms;

public class Entrance : RoomBase
{
    protected override RoomInstruction GetRoomInstructions()
    {
        return new RoomInstruction
        {
            Room = this,
            Dialogue = GameState.FountainActivated
                ? "[blue]The Fountain of Objects has been reactivated, and you have escaped with your life![/]"
                : "You see light in this room coming from outside the cavern. This is the entrance.",
        };
    }

    public override RoomInstruction EnterRoom()
    {
        if (GameState.FountainActivated) GameState.IsGameOver = true;
        return base.EnterRoom();
    }
}
