using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Rooms;

public abstract class RoomBase
{
    public bool Visited { get; set; }
    public required GameState GameState { get; init; }

    protected abstract RoomInstruction GetRoomInstructions();

    public virtual List<Choice> GetRoomActions() => [];

    public virtual string[] HandleRoomAction(Choice choice) => [];

    public virtual RoomInstruction EnterRoom()
    {
        Visited = true;
        return GetRoomInstructions();
    }
}
