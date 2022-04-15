using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

namespace BoardGameGeek.Dungeon.Services
{
    [XmlRoot("plays")]
    public class UserPlays
    {
        [XmlAttribute("userid")]
        public int UserId { get; set; }

        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlAttribute("total")]
        public int Total { get; set; }

        [XmlAttribute("page")]
        public int Page { get; set; }

        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; }

        [XmlElement("play")]
        public PlayItems[] Plays { get; set; }

        public override string ToString() => $"UserName = {UserName}, Total = {Total}, Page = {Page}";
    }

    public class PlayItems
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlAttribute("length")]
        public int Length { get; set; }

        [XmlAttribute("incomplete")]
        public bool Incomplete { get; set; }

        [XmlAttribute("nowinstats")]
        public bool NoWinStats { get; set; }

        [XmlAttribute("location")]
        public string Location { get; set; }

        [XmlElement("comments")]
        public string Comments { get; set; }

        [XmlElement("item")]
        public PlayItem[] Items { get; set; }

        [XmlArray("players")]
        [XmlArrayItem("player", typeof(PlayPlayer))]
        public PlayPlayer[] Players { get; set; }

        public override string ToString() => $"{Date:yyyy-MM-dd}: {Quantity}x {Items.Single().Name}";
    }

    public class PlayItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("objecttype")]
        public string ObjectType { get; set; }

        [XmlAttribute("objectid")]
        public int ObjectId { get; set; }

        [XmlArray("subtypes")]
        [XmlArrayItem("subtype", typeof(PlayItemStringValue))]
        public PlayItemStringValue[] Subtypes { get; set; }

        public override string ToString() => $"Name = {Name}";
    }

    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class PlayItemStringValue
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class PlayPlayer
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlAttribute("userid")]
        public int UserId { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("startposition")]
        public string StartPosition { get; set; }

        [XmlAttribute("color")]
        public string Color { get; set; }

        [XmlAttribute("score")]
        public string Score { get; set; }

        [XmlAttribute("new")]
        public bool New { get; set; }

        [XmlAttribute("rating")]
        public string Rating { get; set; }

        [XmlAttribute("win")]
        public bool Win { get; set; }

        public override string ToString() => $"Name = {Name}, Score = {Score}";
    }
}
