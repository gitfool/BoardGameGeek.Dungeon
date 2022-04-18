namespace BoardGameGeek.Dungeon.Models;

public sealed record Stats
{
    public int Plays { get; set; }
    public int Sessions { get; set; }
    public int Unique { get; set; }
    public int New { get; set; }
    public int Highlights { get; set; }
}
