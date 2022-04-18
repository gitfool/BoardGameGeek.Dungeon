using System.Linq;
using System.Text.RegularExpressions;

namespace BoardGameGeek.Dungeon.Models
{
    public sealed record Game
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public bool IsExpansion { get; init; }
        public int ParentId { get; init; }
        public int Plays { get; init; }
        public int Sessions { get; init; }
        public int CumulativePlays { get; init; }
        public int CumulativeSessions { get; init; }
        public bool IsHighlight { get; init; }
        public bool IsNew { get; init; }
        public string? Comments { get; init; }

        public override string ToString() => $"{Plays}x {Name} (\u03a3 {CumulativePlays}x)";

        internal static bool IsHighlightMatch(string? comments, int? year)
        {
            var highlights = HighlightsMatchRegex.Matches(comments ?? string.Empty)
                .SelectMany(match => HighlightsSplitRegex.Split(match.Value).Select(int.Parse)).ToArray();
            return year != null ? highlights.Contains(year.Value) : highlights.Length > 0;
        }

        private static readonly Regex HighlightsMatchRegex = new(@"(?<=Highlights?:?\s*)\d{4}((,\s*|\s+)\d{4})*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex HighlightsSplitRegex = new(@",\s*|\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
