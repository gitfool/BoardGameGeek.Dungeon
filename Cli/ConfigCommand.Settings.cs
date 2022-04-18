namespace BoardGameGeek.Dungeon;

public sealed partial class ConfigCommand
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--yaml")]
        [Description("Yaml output")]
        public bool Yaml { get; init; }
    }
}
