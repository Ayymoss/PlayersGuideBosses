using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public class Fountain : RoomBase
{
    protected override List<InstructionBase> GetRoomInstructions()
    {
        return
        [
            new DialogueInstruction(this)
            {
                Dialogue = GameState.FountainActivated
                    ? "[green]You hear the rushing waters from the Fountain of Objects. It has been reactivated![/]"
                    : "[blue]You hear water dripping in this room. The Fountain of Objects is here![/]",
            }
        ];
    }

    public override List<Choice> GetRoomActions() => !GameState.FountainActivated ? [Choice.Activate] : [];

    public override string[] HandleRoomAction(Choice choice)
    {
        if (choice is not Choice.Activate) return base.HandleRoomAction(choice);

        if (GameState.FountainActivated) return ["[red]You have already activated the Fountain of Objects[/]"];

        GameState.FountainActivated = true;

        return
        [
            "You have activated the Fountain of Objects and the cavern is now collapsing!",
            "You must escape to the entrance to survive!"
        ];
    }
}
