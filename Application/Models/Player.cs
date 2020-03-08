namespace BoardGameGeek.Dungeon.Models
{
    public class Player
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

        public override string ToString() => $"Name = {Name}, Score = {Score}";
    }
}
