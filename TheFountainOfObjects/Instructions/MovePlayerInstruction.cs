using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public class MovePlayerInstruction(RoomBase roomBase) : InstructionBase(roomBase)
{
    public required Choice Move { get; set; }
}
