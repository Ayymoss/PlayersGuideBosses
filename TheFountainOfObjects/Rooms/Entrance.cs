namespace TheFountainOfObjects.Rooms;

public class Entrance : RoomBase
{
    public event Action? IsFountainActivated;
    public required Fountain Fountain { get; init; }

    public override string RoomDialogue()
    {
        if (Fountain.Activated) IsFountainActivated?.Invoke();

        return Fountain.Activated
            ? "The Fountain of Objects has been reactivated, and you have escaped with your life!"
            : "You see light in this room coming from outside the cavern. This is the entrance.";
    }
}
