using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace BoardGameGeek.Dungeon.Services
{
    [XmlRoot("plays")]
    public sealed record UserPlays
    {
        [XmlAttribute("userid")]
        public int UserId { get; init; }

        [XmlAttribute("username")]
        public string UserName { get; init; } = null!;

        [XmlAttribute("total")]
        public int Total { get; init; }

        [XmlAttribute("page")]
        public int Page { get; init; }

        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; init; } = null!;

        [XmlElement("play")]
        public PlayItems[]? Plays { get; init; }

        public override string ToString() => $"UserName = {UserName}, Total = {Total}, Page = {Page}";
    }

    public sealed record PlayItems
    {
        [XmlAttribute("id")]
        public int Id { get; init; }

        [XmlAttribute("date")]
        public DateTime Date { get; init; }

        [XmlAttribute("quantity")]
        public int Quantity { get; init; }

        [XmlAttribute("length")]
        public int Length { get; init; }

        [XmlAttribute("incomplete")]
        public bool Incomplete { get; init; }

        [XmlAttribute("nowinstats")]
        public bool NoWinStats { get; init; }

        [XmlAttribute("location")]
        public string? Location { get; init; }

        [XmlElement("comments")]
        public string? Comments { get; init; }

        [XmlElement("item")]
        public PlayItem[] Items { get; init; } = null!;

        [XmlArray("players")]
        [XmlArrayItem("player", typeof(PlayPlayer))]
        public PlayPlayer[]? Players { get; init; }

        public override string ToString() => $"{Date:yyyy-MM-dd}: {Quantity}x {Items.Single().Name}";
    }

    public sealed record PlayItem
    {
        [XmlAttribute("name")]
        public string Name { get; init; } = null!;

        [XmlAttribute("objecttype")]
        public string ObjectType { get; init; } = null!;

        [XmlAttribute("objectid")]
        public int ObjectId { get; init; }

        [XmlArray("subtypes")]
        [XmlArrayItem("subtype", typeof(PlayItemStringValue))]
        public PlayItemStringValue[] Subtypes { get; init; } = null!;

        public override string ToString() => $"Name = {Name}";
    }

    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public sealed record PlayItemStringValue
    {
        [XmlAttribute("value")]
        public string Value { get; init; } = null!;
    }

    public sealed record PlayPlayer
    {
        [XmlAttribute("username")]
        public string? UserName { get; init; }

        [XmlAttribute("userid")]
        public int UserId { get; init; }

        [XmlAttribute("name")]
        public string Name { get; init; } = null!;

        [XmlAttribute("startposition")]
        public string? StartPosition { get; init; }

        [XmlAttribute("color")]
        public string? Color { get; init; }

        [XmlAttribute("score")]
        public string Score { get; init; } = null!;

        [XmlAttribute("new")]
        public bool New { get; init; }

        [XmlAttribute("rating")]
        public double Rating { get; init; }

        [XmlAttribute("win")]
        public bool Win { get; init; }

        public override string ToString() => $"Name = {Name}, Score = {Score}";
    }
}
