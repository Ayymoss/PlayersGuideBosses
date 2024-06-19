using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Instructions;

public class MoveRoomInstruction : InstructionBase
{
    public required Choice Move { get; set; }
}
