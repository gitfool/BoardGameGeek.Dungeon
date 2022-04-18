namespace BoardGameGeek.Dungeon.Models
{
    public sealed record Thing
    {
        public int Id { get; init; }
        public string Type { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Image { get; init; } = null!;
        public string Thumbnail { get; init; } = null!;
        public ThingLink[] Links { get; init; } = null!;

        public override string ToString() => $"Type = {Type}, Name = {Name}";
    }
}
