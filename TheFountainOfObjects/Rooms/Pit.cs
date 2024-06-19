using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public class Pit : RoomBase
{
    protected override List<InstructionBase> GetRoomInstructions()
    {
        return
        [
            new DialogueInstruction(this)
            {
                Dialogue = "[red]You have fallen into a pit. You are dead.[/]",
            }
        ];
    }

    public override List<InstructionBase> EnterRoom()
    {
        GameState.IsGameOver = true;
        return base.EnterRoom();
    }
}
