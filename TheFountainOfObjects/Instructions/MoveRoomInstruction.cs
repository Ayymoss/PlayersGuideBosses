using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public class MoveRoomInstruction : InstructionBase
{
    public required Choice Move { get; set; }
}
