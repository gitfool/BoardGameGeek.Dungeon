namespace BoardGameGeek.Dungeon.Models
{
    public sealed record ThingLink
    {
        public int Id { get; init; }
        public string Type { get; init; } = null!;
        public string Value { get; init; } = null!;
        public bool IsExpansion { get; init; }
        public bool IsInbound { get; init; }

        public override string ToString() => $"Type = {Type}, Value = {Value}";
    }
}
