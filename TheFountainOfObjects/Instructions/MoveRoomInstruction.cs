using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects.Instructions;

public class MoveRoomInstruction(RoomBase roomBase) : InstructionBase(roomBase)
{
    public required Choice Move { get; set; }
}
