namespace BoardGameGeek.Dungeon.Models
{
    public sealed record Player
    {
        public int UserId { get; init; }
        public string? UserName { get; init; }
        public string Name { get; init; } = null!;
        public string? StartPosition { get; init; }
        public string? Color { get; init; }
        public string Score { get; init; } = null!;
        public bool New { get; init; }
        public double Rating { get; init; }
        public bool Win { get; init; }

        public override string ToString() => $"Name = {Name}, Score = {Score}";
    }
}
