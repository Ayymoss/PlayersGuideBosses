using TheFountainOfObjects.Enums;
using TheFountainOfObjects.Rooms;

namespace TheFountainOfObjects;

public class Player(byte x = 0, byte y = 0) : Entity(x, y)
{
    public required RoomBase CurrentRoom { get; set; }
    public int Arrows { get; private set; } = 5;

    public string[] ShootArrow()
    {
        Arrows--;

        List<string> response = ["[red1]You shoot an arrow.[/]"];
        if (Arrows is 0) response.Add("[darkorange]You have no arrows.[/]");
        return response.ToArray();
    }

    public IEnumerable<Choice> GetChoices()
    {
        List<Choice> choices =
        [
            Choice.MoveNorth,
            Choice.MoveEast,
            Choice.MoveSouth,
            Choice.MoveWest,
        ];

        if (Arrows > 0)
        {
            choices.AddRange
            (
                [
                    Choice.ShootNorth,
                    Choice.ShootEast,
                    Choice.ShootSouth,
                    Choice.ShootWest
                ]
            );
        }

        return choices;
    }
}
