using Pocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Pocket.Logger<BoardGameGeek.Dungeon.Processor>;
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace BoardGameGeek.Dungeon
{
    public class Processor
    {
        public Processor(IBggService bggService)
        {
            BggService = bggService;
            Summary = new Summary();
        }

        public Task <Summary> ProcessPlays(string userName, int? year)
        {
            Summary.UserPlays = EnumerateUserPlays(userName, year).ToArray();

            return Task.FromResult(Summary);
        }

        public Task<Summary> ProcessStats(string userName, int? year)
        {
            Summary.UserPlays = ProcessPlays(userName, EnumerateUserPlays(userName, year).ToArray());
            Summary.UserGames = EnumerateUserGames(userName, year, Summary.UserPlays).ToArray();

            Summary.BaseGame.Plays = Summary.UserPlays.Where(play => !play.IsExpansion && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity);
            Summary.BaseGame.Sessions = Summary.UserPlays.Where(play => !play.IsExpansion).Sum(play => play.Quantity);
            Summary.BaseGame.Unique = Summary.UserGames.Count(game => !game.IsExpansion);
            Summary.BaseGame.New = Summary.UserGames.Count(game => !game.IsExpansion && game.IsNew);
            Summary.BaseGame.Highlights = Summary.UserGames.Count(game => !game.IsExpansion && game.IsHighlight);

            Summary.Expansion.Plays = Summary.UserPlays.Where(play => play.IsExpansion && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity);
            Summary.Expansion.Sessions = Summary.UserPlays.Where(play => play.IsExpansion).Sum(play => play.Quantity);
            Summary.Expansion.Unique = Summary.UserGames.Count(game => game.IsExpansion);
            Summary.Expansion.New = Summary.UserGames.Count(game => game.IsExpansion && game.IsNew);
            Summary.Expansion.Highlights = Summary.UserGames.Count(game => game.IsExpansion && game.IsHighlight);

            return Task.FromResult(Summary);
        }

        private IEnumerable<ThingDto> EnumerateThings(IEnumerable<int> ids)
        {
            return ids.Distinct()
                .OrderBy(id => id)
                .Buffer(100)
                .Select(batch => BggService.DownloadThingsAsync(batch))
                .SelectMany(items => items.GetAwaiter().GetResult().Items
                    .Select(item => new ThingDto
                    {
                        Id = item.Id,
                        Type = item.Type,
                        Name = item.Names.First().Value,
                        Image = item.Image,
                        Thumbnail = item.Thumbnail,
                        Links = item.Links.Select(link => new ThingLinkDto
                        {
                            Id = link.Id,
                            Type = link.Type,
                            Value = link.Value,
                            IsExpansion = link.Type == "boardgameexpansion",
                            IsInbound = link.IsInbound
                        }).ToArray()
                    }));
        }

        private IEnumerable<CollectionDto> EnumerateUserCollection(string userName)
        {
            return BggService.DownloadUserCollectionAsync(userName).GetAwaiter().GetResult().Items
                .Select(item => new CollectionDto
                {
                    GameId = item.ObjectId,
                    GameName = item.Name,
                    Image = item.Image,
                    Thumbnail = item.Thumbnail,
                    TotalPlays = item.TotalPlays,
                    Comments = item.Comments
                });
        }

        private IEnumerable<GameDto> EnumerateUserGames(string userName, int? year, PlayDto[] plays)
        {
            Log.Info("Enumerating user collection");
            var userCollection = EnumerateUserCollection(userName).ToArray();

            Log.Info("Enumerating user games");
            return plays.GroupBy(play => play.GameId).Select(perGamePlays =>
            {
                var game = new { Id = perGamePlays.Key, Name = perGamePlays.First().GameName, perGamePlays.First().IsExpansion, ParentId = perGamePlays.First().ParentGameId };
                var allGamePlays = EnumerateUserPlays(userName, id: game.Id).ToArray();

                var gameCollection = userCollection.Where(collection => collection.GameId == game.Id).ToArray();
                if (gameCollection.Length == 0) // not really a problem since we only want comments for metadata
                {
                    //Log.Warning("*** Failed to find game in user collection:");
                    //Log.Warning($"    {game.Name}");
                    //Log.Warning($"    boardgamegeek.com/{(game.IsExpansion ? "boardgameexpansion" : "boardgame")}/{game.Id}");
                }
                else if (gameCollection.Length > 1)
                {
                    Log.Warning("*** Found multiple games in user collection:");
                    Log.Warning($"    {game.Name}");
                    Log.Warning($"    boardgamegeek.com/{(game.IsExpansion ? "boardgameexpansion" : "boardgame")}/{game.Id}");
                }
                var gameComments = string.Join(@"\n", gameCollection.Select(collection => collection.Comments?.Replace("\n", @"\n")));

                return new GameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    IsExpansion = game.IsExpansion,
                    ParentId = game.ParentId,
                    Plays = perGamePlays.Where(play => !play.IsSession || !play.IsIncomplete).Sum(play => play.Quantity),
                    Sessions = perGamePlays.Sum(play => play.Quantity),
                    CumulativePlays = allGamePlays.Where(play => (year == null || play.Date.Year <= year) && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity),
                    CumulativeSessions = allGamePlays.Where(play => year == null || play.Date.Year <= year).Sum(play => play.Quantity),
                    IsHighlight = IsHighlight(gameComments, year),
                    IsNew = year != null && allGamePlays.All(play => play.Date.Year >= year),
                    Comments = gameComments
                };
            });
        }

        private IEnumerable<PlayDto> EnumerateUserPlays(string userName, int? year = null, int? id = null)
        {
            if (id == null)
            {
                Log.Info("Enumerating user plays");
            }

            var userPlays = BggService.DownloadUserPlaysAsync(userName, year, id).GetAwaiter().GetResult();
            var totalPlays = userPlays.Total;
            var totalPages = (int)Math.Ceiling(totalPlays / 100d); // get total pages from first page
            for (var page = 1; page <= totalPages;)
            {
                foreach (var play in userPlays.Plays)
                {
                    var item = play.Items.Single();
                    yield return new PlayDto
                    {
                        PlayId = play.Id,
                        GameId = item.ObjectId,
                        GameName = item.Name,
                        IsExpansion = item.SubTypes.Any(subType => subType.Value == "boardgameexpansion"),
                        Date = play.Date,
                        Quantity = play.Quantity,
                        Length = play.Length,
                        IsIncomplete = play.IsIncomplete,
                        IsSession = IsSession(play.Comments),
                        Location = play.Location,
                        Comments = play.Comments,
                    };
                }
                if (page++ < totalPages)
                {
                    userPlays = BggService.DownloadUserPlaysAsync(userName, year, id, page).GetAwaiter().GetResult();
                }
            }
        }

        internal bool IsHighlight(string comments, int? year)
        {
            var highlights = HighlightsMatchRegex.Matches(comments ?? string.Empty)
                .SelectMany(match => HighlightsSplitRegex.Split(match.Value).Select(int.Parse)).ToArray();
            return year != null ? highlights.Contains(year.Value) : highlights.Length > 0;
        }

        internal static bool IsSession(string comments)
        {
            return comments != null && SessionRegex.IsMatch(comments);
        }

        private PlayDto[] ProcessPlays(string userName, PlayDto[] plays)
        {
            Log.Info("Processing user plays");

            // check plays
            foreach (var play in plays.Where(play => play.IsSession && play.Quantity != 1))
            {
                Log.Warning("*** Found multi-play session with unexpected quantity:");
                Log.Warning($"    {play.Date:yyyy-MM-dd} {play.GameName}");
                Log.Warning($"    boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{play.Date.AddDays(-14):yyyy-MM-dd}/end/{play.Date.AddDays(14):yyyy-MM-dd}");
                Log.Warning($"    boardgamegeek.com/{(play.IsExpansion ? "boardgameexpansion" : "boardgame")}/{play.GameId}");
                Log.Trace($">>> {play.Quantity}");
                play.Quantity = 1;
                Log.Trace("<<< Selected 1");
            }

            // correlate expansion plays with parent plays on same day to determine parent game
            EnumerateThings(plays.Where(play => play.IsExpansion).Select(play => play.GameId))
                .ForEach(thing => Things[thing.Id] = thing); // cache expansion things

            foreach (var expansionPlay in plays.Where(play => play.IsExpansion))
            {
                var expansion = Things[expansionPlay.GameId];
                var parentIds = expansion.Links
                    .Where(link => link.IsExpansion && link.IsInbound) // inbound expansion links to parents
                    .Select(link => link.Id)
                    .ToArray();
                if (parentIds.Length == 0)
                {
                    // ignore outbound expansion which links to children; i.e. this is actually a parent play, clarified now by the inbound attribute which wasn't available in the play item
                    expansionPlay.IsExpansion = false;
                    continue;
                }
                var playedParentIds = plays
                    .Where(play => play.Date == expansionPlay.Date) // do not filter expansions since they are hierarchical; e.g. 7 Wonders: Leaders is both an expansion and has expansions
                    .Select(play => play.GameId)
                    .Intersect(parentIds)
                    .ToArray();

                if (playedParentIds.Length == 0)
                {
                    Log.Warning("*** Failed to find parent play for expansion play:");
                    Log.Warning($"    {expansionPlay.Date:yyyy-MM-dd} {expansionPlay.GameName}");
                    Log.Warning($"    boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{expansionPlay.Date.AddDays(-14):yyyy-MM-dd}/end/{expansionPlay.Date.AddDays(14):yyyy-MM-dd}");
                    Log.Warning($"    boardgamegeek.com/boardgameexpansion/{expansionPlay.GameId}");
                    if (parentIds.Length > 1)
                    {
                        Log.Trace(">>> Found multiple linked parent games:");
                        foreach (var parentId in parentIds)
                        {
                            var parent = expansion.Links.Single(link => link.Id == parentId);
                            Log.Trace($"    {parent.Value}");
                            Log.Trace($"    boardgamegeek.com/boardgame/{parent.Id}");
                        }
                        expansionPlay.ParentGameId = parentIds.First();
                        Log.Trace($"<<< Selected {expansionPlay.ParentGameId}");
                    }
                    else
                    {
                        var parent = expansion.Links.Single(link => link.Id == parentIds.Single());
                        Log.Trace(">>> Found single parent game:");
                        Log.Trace($"    {parent.Value}");
                        Log.Trace($"    boardgamegeek.com/boardgame/{parent.Id}");
                        expansionPlay.ParentGameId = parent.Id;
                        Log.Trace($"<<< Selected {expansionPlay.ParentGameId}");
                    }
                }
                else if (playedParentIds.Length > 1)
                {
                    Log.Warning("*** Found multiple parent plays for expansion play:");
                    Log.Warning($"    {expansionPlay.Date:yyyy-MM-dd} {expansionPlay.GameName}");
                    Log.Warning($"    boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{expansionPlay.Date.AddDays(-14):yyyy-MM-dd}/end/{expansionPlay.Date.AddDays(14):yyyy-MM-dd}");
                    Log.Warning($"    boardgamegeek.com/boardgameexpansion/{expansionPlay.GameId}");
                    Log.Trace(">>> Using multiple linked parent games:");
                    foreach (var parentId in playedParentIds)
                    {
                        var parent = expansion.Links.Single(link => link.Id == parentId);
                        Log.Trace($"    {parent.Value}");
                        Log.Trace($"    boardgamegeek.com/boardgame/{parent.Id}");
                    }
                    expansionPlay.ParentGameId = playedParentIds.First();
                    Log.Trace($"<<< Selected {expansionPlay.ParentGameId}");
                }
                else
                {
                    expansionPlay.ParentGameId = playedParentIds.Single();
                }
            }

            return plays;
        }

        public Summary Summary { get; }

        private IBggService BggService { get; }

        private static readonly Regex HighlightsMatchRegex = new Regex(@"(?<=Highlights?:?\s*)\d{4}((,\s*|\s+)\d{4})*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex HighlightsSplitRegex = new Regex(@",\s*|\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SessionRegex = new Regex(@"(?<=Session:?\s*)(\d+)\s*\/\s*(\d+|\?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Dictionary<int, ThingDto> Things = new Dictionary<int, ThingDto>();
    }
}
