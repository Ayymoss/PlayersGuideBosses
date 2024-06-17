namespace TheFountainOfObjects.Rooms;

public class Pit : RoomBase
{
    public event Action? HasFallen;
    public override string RoomDialogue()
    {
        HasFallen?.Invoke();
        return "You have fallen into a pit. You are dead.";
    }
}
