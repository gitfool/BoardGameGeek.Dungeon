using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace BoardGameGeek.Dungeon.Services
{
    [XmlRoot("items")]
    public sealed record ThingItems
    {
        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; init; } = null!;

        [XmlElement("item")]
        public ThingItem[] Items { get; init; } = null!;
    }

    public sealed record ThingItem
    {
        [XmlAttribute("type")]
        public string Type { get; init; } = null!;

        [XmlAttribute("id")]
        public int Id { get; init; }

        [XmlElement("image")]
        public string Image { get; init; } = null!;

        [XmlElement("thumbnail")]
        public string Thumbnail { get; init; } = null!;

        [XmlElement("name")]
        public ThingItemName[] Names { get; init; } = null!;

        [XmlElement("description")]
        public string Description { get; init; } = null!;

        [XmlElement("yearpublished")]
        public ThingItemIntegerValue YearPublished { get; init; } = null!;

        [XmlElement("minplayers")]
        public ThingItemIntegerValue MinPlayers { get; init; } = null!;

        [XmlElement("maxplayers")]
        public ThingItemIntegerValue MaxPlayers { get; init; } = null!;

        [XmlElement("playingtime")]
        public ThingItemIntegerValue PlayingTime { get; init; } = null!;

        [XmlElement("minplaytime")]
        public ThingItemIntegerValue MinPlayTime { get; init; } = null!;

        [XmlElement("maxplaytime")]
        public ThingItemIntegerValue MaxPlayTime { get; init; } = null!;

        [XmlElement("minage")]
        public ThingItemIntegerValue MinAge { get; init; } = null!;

        [XmlElement("link")]
        public ThingItemLink[] Links { get; init; } = null!;

        public override string ToString() => $"Type = {Type}, Name = {Names.First().Value}";
    }

    public sealed record ThingItemName
    {
        [XmlAttribute("type")]
        public string Type { get; init; } = null!;

        [XmlAttribute("sortindex")]
        public int SortIndex { get; init; }

        [XmlAttribute("value")]
        public string Value { get; init; } = null!;

        public override string ToString() => $"Type = {Type}, Value = {Value}";
    }

    public sealed record ThingItemLink
    {
        [XmlAttribute("type")]
        public string Type { get; init; } = null!;

        [XmlAttribute("id")]
        public int Id { get; init; }

        [XmlAttribute("value")]
        public string Value { get; init; } = null!;

        [XmlAttribute("inbound")]
        public bool Inbound { get; init; }

        public override string ToString() => $"Type = {Type}, Value = {Value}";
    }

    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public sealed record ThingItemIntegerValue
    {
        [XmlAttribute("value")]
        public int Value { get; init; }
    }
}
