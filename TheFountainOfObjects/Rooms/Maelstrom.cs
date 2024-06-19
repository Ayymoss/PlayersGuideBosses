using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Instructions;

namespace TheFountainOfObjects.Rooms;

public class Maelstrom : RoomBase
{
    protected override List<InstructionBase> GetRoomInstructions()
    {
        return
        [
            new MoveRoomInstruction
            {
                Move = Choice.MoveSouth,
            },
            new MoveRoomInstruction
            {
                Move = Choice.MoveWest,
            },
            new MoveRoomInstruction
            {
                Move = Choice.MoveWest
            },

            new MovePlayerInstruction
            {
                Move = Choice.MoveNorth,
            },
            new MovePlayerInstruction
            {
                Move = Choice.MoveEast,
            },
            new MovePlayerInstruction
            {
                Move = Choice.MoveEast
            },


            new DialogueInstruction
            {
                Dialogue = "[red]You have been sucked into the maelstrom and blown away![/]"
            }
        ];
    }

    public override string[] AdjacentRoomCheck() => ["[yellow]You can hear the whirling of a maelstrom nearby.[/]"];
}
