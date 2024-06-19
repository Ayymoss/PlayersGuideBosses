using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public class MovePlayerInstruction : InstructionBase
{
    public required Choice Move { get; set; }
}
