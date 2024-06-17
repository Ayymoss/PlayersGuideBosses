namespace TheFountainOfObjects.Rooms;

public abstract class RoomBase
{
    public bool Visited { get; set; }

    public abstract string RoomDialogue();
}
