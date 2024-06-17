namespace TheFountainOfObjects.Rooms;

public class Fountain : RoomBase
{
    public bool Activated { get; set; }

    public override string RoomDialogue() => Activated
        ? "You hear the rushing waters from the Fountain of Objects. It has been reactivated!"
        : "You hear water dripping in this room. The Fountain of Objects is here!";
}
