using System.Linq;
using System.Text.RegularExpressions;

namespace BoardGameGeek.Dungeon.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsExpansion { get; set; }
        public int ParentId { get; set; }
        public int Plays { get; set; }
        public int Sessions { get; set; }
        public int CumulativePlays { get; set; }
        public int CumulativeSessions { get; set; }
        public bool IsHighlight { get; set; }
        public bool IsNew { get; set; }
        public string Comments { get; set; }

        public override string ToString() => $"{Plays}x {Name} (\u03a3 {CumulativePlays}x)";

        internal static bool IsHighlightMatch(string comments, int? year)
        {
            var highlights = HighlightsMatchRegex.Matches(comments ?? string.Empty)
                .SelectMany(match => HighlightsSplitRegex.Split(match.Value).Select(int.Parse)).ToArray();
            return year != null ? highlights.Contains(year.Value) : highlights.Length > 0;
        }

        private static readonly Regex HighlightsMatchRegex = new Regex(@"(?<=Highlights?:?\s*)\d{4}((,\s*|\s+)\d{4})*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex HighlightsSplitRegex = new Regex(@",\s*|\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
