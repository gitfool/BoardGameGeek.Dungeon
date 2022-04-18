namespace BoardGameGeek.Dungeon.Models;

public sealed record Play
{
    public int PlayId { get; set; }
    public int GameId { get; init; }
    public string? GameName { get; init; }
    public bool IsExpansion { get; set; }
    public int ParentGameId { get; set; }
    public DateTime Date { get; init; }
    public int Quantity { get; set; }
    public int Length { get; init; }
    public bool IsIncomplete { get; init; }
    public bool IsSession { get; init; }
    public bool NoWinStats { get; init; }
    public string? Location { get; init; }
    public string? Comments { get; init; }
    public Player[]? Players { get; init; }

    public override string ToString() => $"{Date:yyyy-MM-dd}: {Quantity}x {GameName ?? GameId.ToString()}";

    internal static bool IsSessionMatch(string? comments) => comments != null && SessionRegex.IsMatch(comments);

    private static readonly Regex SessionRegex = new(@"(?<=Session:?\s*)(\d+)\s*\/\s*(\d+|\?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
}
