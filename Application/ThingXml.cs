using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

namespace BoardGameGeek.Dungeon
{
    [XmlRoot("items")]
    public class ThingItems
    {
        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; }

        [XmlElement("item")]
        public ThingItem[] Items { get; set; }
    }

    public class ThingItem
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; }

        [XmlElement("name")]
        public ThingItemName[] Names { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("yearpublished")]
        public ThingItemIntegerValue YearPublished { get; set; }

        [XmlElement("minplayers")]
        public ThingItemIntegerValue MinPlayers { get; set; }

        [XmlElement("maxplayers")]
        public ThingItemIntegerValue MaxPlayers { get; set; }

        [XmlElement("playingtime")]
        public ThingItemIntegerValue PlayingTime { get; set; }

        [XmlElement("minplaytime")]
        public ThingItemIntegerValue MinPlayTime { get; set; }

        [XmlElement("maxplaytime")]
        public ThingItemIntegerValue MaxPlayTime { get; set; }

        [XmlElement("minage")]
        public ThingItemIntegerValue MinAge { get; set; }

        [XmlElement("link")]
        public ThingItemLink[] Links { get; set; }

        public override string ToString()
        {
            return $"Type = {Type}, Name = {Names.First().Value}";
        }
    }

    public class ThingItemName
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("sortindex")]
        public int SortIndex { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Type = {Type}, Value = {Value}";
        }
    }

    public class ThingItemLink
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("inbound")]
        public bool IsInbound { get; set; }

        public override string ToString()
        {
            return $"Type = {Type}, Value = {Value}";
        }
    }

    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class ThingItemIntegerValue
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }
}
