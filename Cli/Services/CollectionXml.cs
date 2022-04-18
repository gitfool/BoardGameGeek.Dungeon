using System.Text;
using System.Xml.Serialization;

namespace BoardGameGeek.Dungeon.Services
{
    [XmlRoot("items")]
    public sealed record CollectionItems
    {
        [XmlAttribute("totalitems")]
        public int TotalItems { get; init; }

        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; init; } = null!;

        [XmlAttribute("pubdate")]
        public string PubDate { get; init; } = null!;

        [XmlElement("item")]
        public CollectionItem[] Items { get; init; } = null!;
    }

    public sealed record CollectionItem
    {
        [XmlAttribute("objecttype")]
        public string ObjectType { get; init; } = null!;

        [XmlAttribute("objectid")]
        public int ObjectId { get; init; }

        [XmlAttribute("subtype")]
        public string Subtype { get; init; } = null!;

        [XmlElement("name")]
        public string Name { get; init; } = null!;

        [XmlElement("yearpublished")]
        public int YearPublished { get; init; }

        [XmlElement("image")]
        public string Image { get; init; } = null!;

        [XmlElement("thumbnail")]
        public string Thumbnail { get; init; } = null!;

        [XmlElement("numplays")]
        public int NumPlays { get; init; }

        [XmlElement("comment")]
        public string? Comments { get; init; }

        [XmlElement("status")]
        public CollectionItemStatus Status { get; init; } = null!;

        public override string ToString() => $"Name = {Name}, NumPlays = {NumPlays}";
    }

    public sealed record CollectionItemStatus
    {
        [XmlAttribute("own")]
        public bool Own { get; init; }

        [XmlAttribute("prevowned")]
        public bool PrevOwned { get; init; }

        [XmlAttribute("fortrade")]
        public bool ForTrade { get; init; }

        [XmlAttribute("want")]
        public bool WantInTrade { get; init; }

        [XmlAttribute("wanttoplay")]
        public bool WantToPlay { get; init; }

        [XmlAttribute("wanttobuy")]
        public bool WantToBuy { get; init; }

        [XmlAttribute("wishlist")]
        public bool WishList { get; init; }

        [XmlAttribute("wishlistpriority")]
        public int WishListPriority { get; init; }

        [XmlAttribute("preordered")]
        public bool Preordered { get; init; }

        [XmlAttribute("lastmodified")]
        public string LastModified { get; init; } = null!;

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Own)
            {
                builder.Append(nameof(Own));
            }
            if (PrevOwned)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(PrevOwned));
            }
            if (ForTrade)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(ForTrade));
            }
            if (WantInTrade)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(WantInTrade));
            }
            if (WantToPlay)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(WantToPlay));
            }
            if (WantToBuy)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(WantToBuy));
            }
            if (WishList)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append($"{nameof(WishList)}({WishListPriority})");
            }
            if (Preordered)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(nameof(Preordered));
            }
            return builder.ToString();
        }
    }
}
