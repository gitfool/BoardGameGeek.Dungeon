using System;
using System.Text.RegularExpressions;

namespace BoardGameGeek.Dungeon.Models
{
    public class Play
    {
        public int PlayId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public bool IsExpansion { get; set; }
        public int ParentGameId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public int Length { get; set; }
        public bool IsIncomplete { get; set; }
        public bool IsSession { get; set; }
        public bool NoWinStats { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public Player[] Players { get; set; }

        public override string ToString() => $"{Date:yyyy-MM-dd}: {Quantity}x {GameName ?? GameId.ToString()}";

        internal static bool IsSessionMatch(string comments) => comments != null && SessionRegex.IsMatch(comments);

        private static readonly Regex SessionRegex = new Regex(@"(?<=Session:?\s*)(\d+)\s*\/\s*(\d+|\?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
