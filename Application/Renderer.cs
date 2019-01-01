using Pocket;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pocket.Logger<BoardGameGeek.Dungeon.Renderer>;
// ReSharper disable StringLiteralTypo

namespace BoardGameGeek.Dungeon
{
    public class Renderer
    {
        public async Task RenderPlays(string userName, int? year, Summary summary)
        {
            var fileName = $"BGG-{userName}-Plays-{year ?? 0:D4}.csv";
            Log.Info($"Writing file {fileName}");

            using (var file = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                await file.WriteLineAsync(@"Date,Location,Quantity,Game,Length,Incomplete,Comments");
                foreach (var play in summary.UserPlays)
                {
                    await file.WriteLineAsync($@"{play.Date:yyyy-MM-dd},""{play.Location}"",{play.Quantity},""{play.GameName}"",{play.Length},{(play.IsIncomplete ? "Y" : "")},""{play.Comments?.Replace("\n", @"\n")}""");
                }
            }
        }

        public async Task RenderStats(string userName, int? year, Summary summary)
        {
            var fileName = $"BGG-{userName}-Plays-{year ?? 0:D4}-Summary.txt";
            Log.Info($"Writing file {fileName}");

            using (var file = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                // write summary
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
                foreach (var baseGame in summary.UserGames
                    .Where(game => !game.IsExpansion)
                    .OrderByDescending(game => game.Plays)
                    .ThenBy(game => game.Name))
                {
                    await file.WriteLineAsync($"[color=gray]{baseGame.Plays}x[/color] [thing={baseGame.Id}]{Highlight(baseGame.Name, baseGame.IsHighlight)}[/thing] {Suffix(baseGame)}");
                    foreach (var expansion in summary.UserGames
                        .Where(game => game.IsExpansion && game.ParentId == baseGame.Id)
                        .OrderByDescending(game => game.Plays)
                        .ThenBy(game => game.Name))
                    {
                        await file.WriteLineAsync($"[c]    [/c][color=gray]{expansion.Plays}x[/color]  [thing={expansion.Id}]{Highlight(expansion.Name, expansion.IsHighlight)}[/thing] {Suffix(expansion)}");
                    }
                }
                await file.WriteLineAsync();
            }
        }

        private static string Highlight(string text, bool isHighlight = true)
        {
            return isHighlight ? $"[bgcolor=gold]{text}[/bgcolor]" : text;
        }

        private static string Pluralize(int count)
        {
            return count != 1 ? "s" : string.Empty;
        }

        private static string Star(int count)
        {
            return count >= 100 ? ":star:" : count >= 10 ? ":halfstar:" : ":nostar:";
        }

        private static string Suffix(GameDto game)
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
    }
}
