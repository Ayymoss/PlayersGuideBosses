using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public abstract class InstructionBase(RoomBase roomBase)
{
    public RoomBase Room { get; } = roomBase;
}
