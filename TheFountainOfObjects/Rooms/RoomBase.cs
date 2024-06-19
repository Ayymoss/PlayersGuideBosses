using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public abstract class RoomBase
{
    public bool Visited { get; set; }
    public required GameState GameState { get; init; }

    protected abstract List<InstructionBase> GetRoomInstructions();

    public virtual List<Choice> GetRoomActions() => [];

    public virtual string[] HandleRoomAction(Choice choice) => [];

    public virtual string[] AdjacentRoomCheck() => [];

    public virtual List<InstructionBase> EnterRoom()
    {
        Visited = true;
        return GetRoomInstructions();
    }
}
