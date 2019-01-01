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
        public string Location { get; set; }
        public string Comments { get; set; }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd}: {Quantity}x {GameName}";
        }
    }
}
