using System.ComponentModel;
using Spectre.Console.Cli;

namespace BoardGameGeek.Dungeon;

public sealed partial class LoginCommand
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<username>")]
        [Description("Geek username")]
        public string UserName { get; init; } = null!;

        [CommandArgument(1, "[password]")]
        [Description("Geek password")]
        public string? Password { get; init; }
    }
}
