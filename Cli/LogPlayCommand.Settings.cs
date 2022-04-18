using System;
using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace BoardGameGeek.Dungeon;

public sealed partial class LogPlayCommand
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<username>")]
        [Description("Geek username")]
        public string UserName { get; init; } = null!;

        [CommandOption("-p|--password")]
        [Description("Geek password; defaults to last specified")]
        public string? Password { get; init; }

        [CommandOption("-d|--date")]
        [Description("Date; defaults to current date")]
        public DateTime Date { get; init; } = DateTime.Now.Date;

        [CommandOption("-l|--location")]
        [Description("Location; defaults to unspecified")]
        public string? Location { get; init; }

        [CommandOption("-q|--quantity")]
        [Description("Quantity; defaults to 1")]
        public int Quantity { get; init; } = 1;

        [CommandOption("-g|--game-id")]
        [Description("Game id; defaults to unspecified")]
        public int? GameId { get; init; }

        [CommandOption("--game-name")]
        [Description("Game name; defaults to unspecified")]
        public string? GameName { get; init; }

        [CommandOption("--length")]
        [Description("Length (minutes); defaults to 0")]
        public int Length { get; init; }

        [CommandOption("--incomplete")]
        [Description("Incomplete; defaults to false")]
        public bool Incomplete { get; init; }

        [CommandOption("--no-win-stats")]
        [Description("No win stats; defaults to false")]
        public bool NoWinStats { get; init; }

        [CommandOption("--comments")]
        [Description("Comments; defaults to unspecified")]
        public string? Comments { get; init; }

        public override ValidationResult Validate() => this is { GameId: null, GameName: null } or { GameId: { }, GameName: { } }
            ? ValidationResult.Error("Only one of game id or game name must be specified.")
            : ValidationResult.Success();
    }
}
