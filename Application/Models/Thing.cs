namespace BoardGameGeek.Dungeon.Models
{
    public class Thing
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public ThingLink[] Links { get; set; }

        public override string ToString() => $"Type = {Type}, Name = {Name}";
    }
}
