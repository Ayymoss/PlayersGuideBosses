using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public class Amarok : RoomBase
{
    private bool _isKilled;

    protected override List<InstructionBase> GetRoomInstructions()
    {
        return
        [
            new DialogueInstruction(this)
            {
                Dialogue = _isKilled ? "[yellow]You find a dead wolf.[/]" : "[red]You are in a room with a large wolf. You died.[/]"
            }
        ];
    }

    public override string[] AdjacentRoomCheck() => _isKilled ? [] : ["You can smell the rotten stench of an amarok in a nearby room."];

    public override List<InstructionBase> EnterRoom()
    {
        if (!_isKilled) GameState.IsGameOver = true;
        return base.EnterRoom();
    }

    public override string[] HandleRoomAction(Choice choice)
    {
        if (choice is not Choice.Attack) return base.HandleRoomAction(choice);

        _isKilled = true;
        return ["[green]You have killed a amarok![/]"];
    }
}
