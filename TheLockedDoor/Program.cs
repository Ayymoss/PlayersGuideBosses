using Humanizer;
using Spectre.Console;
using TheLockedDoor.Enums;

namespace TheLockedDoor;

public static class Program
{
    private static Door _door = null!;

    public static void Main()
    {
        var password = AskDoorPassword();
        _door = new Door(password);
        WalkUpToTheDoor();
    }

    private static void WalkUpToTheDoor()
    {
        var atTheDoor = true;
        while (atTheDoor)
        {
            PrintStatus();

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What do you want to do?")
                    .AddChoices([
                        UserAction.ToggleDoor.Humanize().Titleize(),
                        UserAction.ToggleLock.Humanize().Titleize(),
                        UserAction.SetCode.Humanize().Titleize(),
                        UserAction.Exit.Humanize().Titleize()
                    ]));
            var actionParsed = action.DehumanizeTo<UserAction>();

            switch (actionParsed)
            {
                case UserAction.ToggleDoor:
                    var doorResult = _door.ToggleDoor();
                    if (!doorResult)
                    {
                        AnsiConsole.MarkupLine("[red]The door is locked[/]");
                        break;
                    }

                    AnsiConsole.MarkupLine(_door.DoorState is DoorState.Open
                        ? "The door is now [green]open[/]"
                        : "The door is now [red]closed[/]");
                    break;
                case UserAction.ToggleLock:
                    var password = AskDoorPassword();
                    var lockResult = _door.ToggleLock(password);
                    if (!lockResult)
                    {
                        AnsiConsole.MarkupLine("[red]Incorrect password[/]");
                        break;
                    }

                    AnsiConsole.MarkupLine(_door.LockState is LockState.Locked
                        ? "[red]The door is locked[/]"
                        : "[green]The door is unlocked[/]");
                    break;
                case UserAction.SetCode:
                    var oldPassword = AskDoorPassword("Enter [red]old[/] password:");
                    var newPassword = AskDoorPassword("Enter [blue]new[/] password:");
                    var codeResult = _door.SetPassword(oldPassword, newPassword);
                    if (!codeResult)
                    {
                        AnsiConsole.MarkupLine("[red]Incorrect password[/]");
                        break;
                    }

                    AnsiConsole.MarkupLine("[green]Password changed[/]");
                    break;
                case UserAction.Exit:
                    atTheDoor = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!atTheDoor) break;
        }

        AnsiConsole.MarkupLine("[yellow]You walked away from the door[/]");
    }

    private static void PrintStatus()
    {
        Console.WriteLine("----------------------------------");
        AnsiConsole.MarkupLine($"[blue]Door[/]: {_door.DoorState} | [blue]Lock[/]: {_door.LockState}");
    }

    private static string AskDoorPassword(string? message = null) => AnsiConsole.Prompt(
        new TextPrompt<string>(string.IsNullOrWhiteSpace(message) ? "Enter door [green]password[/]?" : message)
            .PromptStyle("red")
            .Secret());
}
