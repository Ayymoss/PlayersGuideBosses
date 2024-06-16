using Manticore.Actors;
using Manticore.Enums;
using Manticore.Utilities;
using Spectre.Console;

namespace Manticore;

public class Game
{
    private Actors.Manticore _manticore = null!;
    private Player _player = null!;

    private int _round;

    private readonly Dictionary<RoundType, int> _damageLookup = new()
    {
        { RoundType.Normal, 1 },
        { RoundType.Fire, 3 },
        { RoundType.Electric, 5 },
        { RoundType.FireElectric, 10 }
    };

    public void Start()
    {
        AnsiConsole.MarkupLine("[blue]City Defence[/]: The [red]Manticore[/] is attacking the city! Your turn to defend.");

        while (true)
        {
            if (ShouldGameEnd()) break;

            PrintStatus();

            var cannonRound = CalculateRoundType();
            var cannonDamage = _damageLookup[cannonRound];

            AnsiConsole.MarkupLine($"The cannon is loaded with [yellow]{cannonRound}[/] ammunition and deals [red]{cannonDamage}[/] damage.");
            var distance = "[blue]City Defence[/]: Enter the distance to attack the [red]manticore[/]:".IntHelper();

            HandlePlayerInput(distance, cannonDamage);
            _round++;
        }

        AnsiConsole.MarkupLine(_manticore.Health <= 0
            ? "[green]The manticore has been defeated![/]"
            : "[red]The manticore has destroyed the city![/]");
        AnsiConsole.MarkupLine("Game Over.");
    }

    private void HandlePlayerInput(int distance, int cannonDamage)
    {
        if (distance == _manticore.Distance)
        {
            AnsiConsole.MarkupLine("[blue]City Defence[/]: The cannon [green]hit[/] the [red]manticore[/]!");
            _manticore.Damage(cannonDamage);
            return;
        }

        const string shortMessage = "[blue]City Defence[/]: The cannonball [orangered1]fell short[/] of the [red]manticore[/].";
        const string longMessage = "[blue]City Defence[/]: The cannonball [orangered1]fell long[/] the [red]manticore[/].";
        AnsiConsole.MarkupLine(distance < _manticore.Distance ? shortMessage : longMessage);
        _player.Damage(1);
    }

    public void Setup()
    {
        _round = 1;
        _player = new Player();

        var distance = "[red]Manticore Player[/]: What distance is the manticore?".IntHelper();
        Console.Clear();

        _manticore = new Actors.Manticore { Distance = distance };
    }

    private void PrintStatus()
    {
        AnsiConsole.MarkupLine("------------------------------------------------");
        AnsiConsole.MarkupLine($"STATUS: [yellow]Round[/]: {_round} | " +
                               $"[blue]City[/]: {_player.Health}/{_player.InitialHealth} | " +
                               $"[red]Manticore[/]: {_manticore.Health}/{_manticore.InitialHealth} | ");
    }

    private bool ShouldGameEnd() => _manticore.Health <= 0 || _player.Health <= 0;

    private RoundType CalculateRoundType()
    {
        if (_round % 3 is 0 && _round % 5 is 0) return RoundType.FireElectric;
        if (_round % 3 is 0) return RoundType.Fire;
        return _round % 5 is 0 ? RoundType.Electric : RoundType.Normal;
    }
}
