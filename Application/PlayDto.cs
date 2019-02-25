using System;

namespace BoardGameGeek.Dungeon
{
    public class PlayDto
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
        public PlayPlayerDto[] Players { get; set; }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd}: {Quantity}x {GameName}";
        }
    }

    public class PlayPlayerDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string StartPosition { get; set; }
        public string Color { get; set; }
        public string Score { get; set; }
        public bool New { get; set; }
        public string Rating { get; set; }
        public bool Win { get; set; }

        public override string ToString()
        {
            return $"Name = {Name}, Score = {Score}";
        }
    }
}
