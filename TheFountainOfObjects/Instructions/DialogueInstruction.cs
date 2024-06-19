using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public class DialogueInstruction(RoomBase roomBase) : InstructionBase(roomBase)
{
    public required string Dialogue { get; set; }
}
