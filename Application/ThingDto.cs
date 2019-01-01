namespace BoardGameGeek.Dungeon
{
    public class ThingDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public ThingLinkDto[] Links { get; set; }

        public override string ToString()
        {
            return $"Type = {Type}, Name = {Name}";
        }
    }

    public class ThingLinkDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsExpansion { get; set; }
        public bool IsInbound { get; set; }

        public override string ToString()
        {
            return $"Type = {Type}, Value = {Value}";
        }
    }
}
