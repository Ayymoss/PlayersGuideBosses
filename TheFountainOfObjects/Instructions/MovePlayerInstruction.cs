using TheFountainOfObjects.Enums;

namespace TheFountainOfObjects.Instructions;

public class MovePlayerInstruction : InstructionBase
{
    public required Choice Move { get; set; }
}
