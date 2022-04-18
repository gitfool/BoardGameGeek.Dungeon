namespace BoardGameGeek.Dungeon.Models;

public sealed record Collection
{
    public int GameId { get; init; }
    public string GameName { get; init; } = null!;
    public string Image { get; init; } = null!;
    public string Thumbnail { get; init; } = null!;
    public int TotalPlays { get; init; }
    public string? Comments { get; init; }

    public override string ToString() => $"{GameName} (\u03a3 {TotalPlays}x)";
}
