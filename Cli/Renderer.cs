namespace BoardGameGeek.Dungeon;

public sealed class Renderer
{
    public Renderer(ILogger<Renderer> logger)
    {
        Logger = logger;
    }

    public async Task RenderPlays(string userName, int? year, Summary summary)
    {
        var fileName = $"BGG-{userName}-Plays-{year ?? 0:D4}.csv";
        Logger.LogInformation($"Writing file {fileName}");

        await using var file = new StreamWriter(fileName, false, Encoding.UTF8);
        await file.WriteLineAsync(@"PlayId,Date,Location,Quantity,GameId,GameName,Length,Incomplete,NoWinStats,Comments,Players"); //TODO save players as json using csv helper
        foreach (var play in summary.Plays)
        {
            await file.WriteLineAsync($@"{play.PlayId},{play.Date:yyyy-MM-dd},""{play.Location}"",{play.Quantity},{play.GameId},""{play.GameName}"",{play.Length},{(play.IsIncomplete ? "Y" : "")},{(play.NoWinStats ? "Y" : "")},""{play.Comments?.Replace("\n", @"\n")}"",""{Players(play.Players)}""");
        }
    }

    public async Task RenderStats(string userName, int? year, Summary summary)
    {
        var fileName = $"BGG-{userName}-Plays-{year ?? 0:D4}-Summary.txt";
        Logger.LogInformation($"Writing file {fileName}");

        // write summary
        await using var file = new StreamWriter(fileName, false, Encoding.UTF8);
        await file.WriteLineAsync($"{Star(summary.BaseGame.Plays + summary.Expansion.Plays)} {summary.BaseGame.Plays + summary.Expansion.Plays} play{Pluralize(summary.BaseGame.Plays + summary.Expansion.Plays)}; " +
            $"{summary.BaseGame.Plays} play{Pluralize(summary.BaseGame.Plays)} of {summary.BaseGame.Unique} unique game{Pluralize(summary.BaseGame.Unique)}; " +
            $"{summary.Expansion.Plays} play{Pluralize(summary.Expansion.Plays)} of {summary.Expansion.Unique} unique expansion{Pluralize(summary.Expansion.Unique)}.");
        if (summary.BaseGame.Sessions + summary.Expansion.Sessions > summary.BaseGame.Plays + summary.Expansion.Plays)
        {
            await file.WriteLineAsync($"{Star(summary.BaseGame.Sessions + summary.Expansion.Sessions)} {summary.BaseGame.Sessions + summary.Expansion.Sessions} session{Pluralize(summary.BaseGame.Sessions + summary.Expansion.Sessions)}; " +
                $"{summary.BaseGame.Sessions} session{Pluralize(summary.BaseGame.Sessions)} of {summary.BaseGame.Unique} unique game{Pluralize(summary.BaseGame.Unique)}; " +
                $"{summary.Expansion.Sessions} session{Pluralize(summary.Expansion.Sessions)} of {summary.Expansion.Unique} unique expansion{Pluralize(summary.Expansion.Unique)}.");
        }
        await file.WriteLineAsync($"{Star(summary.BaseGame.New + summary.Expansion.New)} {summary.BaseGame.New + summary.Expansion.New} new (:star:); " +
            $"{summary.BaseGame.New} new game{Pluralize(summary.BaseGame.New)}; " +
            $"{summary.Expansion.New} new expansion{Pluralize(summary.Expansion.New)}.");
        await file.WriteLineAsync($"{Star(summary.BaseGame.Highlights + summary.Expansion.Highlights)} {summary.BaseGame.Highlights + summary.Expansion.Highlights} {Highlight($"highlight{Pluralize(summary.BaseGame.Highlights + summary.Expansion.Highlights)}")}; " +
            $"{summary.BaseGame.Highlights} highlight game{Pluralize(summary.BaseGame.Highlights)}; " +
            $"{summary.Expansion.Highlights} highlight expansion{Pluralize(summary.Expansion.Highlights)}.");
        await file.WriteLineAsync();

        // write base games and expansions
        foreach (var baseGame in summary.Games
                     .Where(game => !game.IsExpansion)
                     .OrderByDescending(game => game.Plays)
                     .ThenBy(game => game.Name))
        {
            await file.WriteLineAsync($"[color=gray]{baseGame.Plays}x[/color] [thing={baseGame.Id}]{Highlight(baseGame.Name!, baseGame.IsHighlight)}[/thing] {Suffix(baseGame)}");
            foreach (var expansion in summary.Games
                         .Where(game => game.IsExpansion && game.ParentId == baseGame.Id)
                         .OrderByDescending(game => game.Plays)
                         .ThenBy(game => game.Name))
            {
                await file.WriteLineAsync($"[c]    [/c][color=gray]{expansion.Plays}x[/color]  [thing={expansion.Id}]{Highlight(expansion.Name!, expansion.IsHighlight)}[/thing] {Suffix(expansion)}");
            }
        }
        await file.WriteLineAsync();
    }

    private static string Players(IEnumerable<Player>? players)
    {
        return players != null ? string.Join(",", players.OrderByDescending(player => player.Score).Select(player => player.Name)) : string.Empty;
    }

    private static string Highlight(string text, bool isHighlight = true) => isHighlight ? $"[bgcolor=gold]{text}[/bgcolor]" : text;

    private static string Pluralize(int count) => count != 1 ? "s" : string.Empty;

    private static string Star(int count) => count >= 100 ? ":star:" : count >= 10 ? ":halfstar:" : ":nostar:";

    private static string Suffix(Game game)
    {
        var builder = new StringBuilder();

        if (game.IsNew)
        {
            builder.Append(":star:");
            if (game.Sessions > game.Plays)
            {
                builder.Append($" [color=lightgray](\u03a3 {game.Sessions}x)[/color]");
            }
        }
        else
        {
            builder.Append("[color=lightgray]");
            if (game.Sessions > game.Plays)
            {
                builder.Append($"(\u03a3 {game.Sessions}x) ");
            }
            builder.Append($"\u03a3 {game.CumulativePlays}x");
            if (game.CumulativeSessions > game.CumulativePlays)
            {
                builder.Append($" (\u03a3 {game.CumulativeSessions}x)");
            }
            builder.Append("[/color]");
        }

        return builder.ToString();
    }

    private ILogger Logger { get; }
}
