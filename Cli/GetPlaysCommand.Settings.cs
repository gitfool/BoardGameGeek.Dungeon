namespace BoardGameGeek.Dungeon;

public sealed partial class GetPlaysCommand
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<username>")]
        [Description("Geek username")]
        public string UserName { get; init; } = null!;

        [CommandOption("-a|--all")]
        [Description("Analyze all override; defaults to false")]
        public bool All { get; init; }

        [CommandOption("-y|--year")]
        [Description("Year to analyze; defaults to current year")]
        public int? Year { get; init; } = DateTime.Now.Year;
    }
}
