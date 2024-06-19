using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public class Maelstrom : RoomBase
{
    protected override List<InstructionBase> GetRoomInstructions()
    {
        return
        [
            new MoveRoomInstruction(this)
            {
                Move = Choice.MoveSouth,
            },
            new MoveRoomInstruction(this)
            {
                Move = Choice.MoveWest,
            },
            new MoveRoomInstruction(this)
            {
                Move = Choice.MoveWest
            },

            new MovePlayerInstruction(this)
            {
                Move = Choice.MoveNorth,
            },
            new MovePlayerInstruction(this)
            {
                Move = Choice.MoveEast,
            },
            new MovePlayerInstruction(this)
            {
                Move = Choice.MoveEast
            },


            new DialogueInstruction(this)
            {
                Dialogue = "[red]You have been sucked into the maelstrom and blown away![/]"
            }
        ];
    }

    public override string[] AdjacentRoomCheck() => ["[yellow]You can hear the whirling of a maelstrom nearby.[/]"];
}
