namespace BoardGameGeek.Dungeon
{
    public class GameDto
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

        public override string ToString()
        {
            return $"{Plays}x {Name} (\u03a3 {CumulativePlays}x)";
        }
    }
}
