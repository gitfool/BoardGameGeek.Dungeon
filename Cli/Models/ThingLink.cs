namespace BoardGameGeek.Dungeon.Models
{
    public class ThingLink
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsExpansion { get; set; }
        public bool IsInbound { get; set; }

        public override string ToString() => $"Type = {Type}, Value = {Value}";
    }
}
