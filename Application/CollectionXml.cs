using System.Text;
using System.Xml.Serialization;
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

namespace BoardGameGeek.Dungeon
{
    [XmlRoot("items")]
    public class CollectionItems
    {
        [XmlAttribute("totalitems")]
        public int TotalItems { get; set; }

        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; }

        [XmlAttribute("pubdate")]
        public string PubDate { get; set; }

        [XmlElement("item")]
        public CollectionItem[] Items { get; set; }
    }

    public class CollectionItem
    {
        [XmlAttribute("objecttype")]
        public string ObjectType { get; set; }

        [XmlAttribute("objectid")]
        public int ObjectId { get; set; }

        [XmlAttribute("subtype")]
        public string SubType { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("yearpublished")]
        public int YearPublished { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; }

        [XmlElement("numplays")]
        public int TotalPlays { get; set; }

        [XmlElement("comment")]
        public string Comments { get; set; }

        [XmlElement("status")]
        public CollectionItemStatus Status { get; set; }

        public override string ToString()
        {
            return $"Name = {Name}, TotalPlays = {TotalPlays}";
        }
    }

    public class CollectionItemStatus
    {
        [XmlAttribute("own")]
        public bool Own { get; set; }

        [XmlAttribute("prevowned")]
        public bool PrevOwned { get; set; }

        [XmlAttribute("fortrade")]
        public bool ForTrade { get; set; }

        [XmlAttribute("want")]
        public bool WantInTrade { get; set; }

        [XmlAttribute("wanttoplay")]
        public bool WantToPlay { get; set; }

        [XmlAttribute("wanttobuy")]
        public bool WantToBuy { get; set; }

        [XmlAttribute("wishlist")]
        public bool WishList { get; set; }

        [XmlAttribute("wishlistpriority")]
        public int WishListPriority { get; set; }

        [XmlAttribute("preordered")]
        public bool Preordered { get; set; }

        [XmlAttribute("lastmodified")]
        public string LastModified { get; set; }

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
