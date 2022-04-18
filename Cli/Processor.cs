namespace BoardGameGeek.Dungeon;

public sealed class Processor
{
    public Processor(ILogger<Processor> logger, IBggService bggService)
    {
        Logger = logger;
        BggService = bggService;
        Summary = new Summary();
    }

    public Summary ProcessPlays(string userName, int? year)
    {
        Summary.Plays = GetUserPlays(userName, year);

        return Summary;
    }

    public Summary ProcessStats(string userName, int? year)
    {
        Summary.Plays = ProcessPlays(userName, GetUserPlays(userName, year));
        Summary.Games = GetUserGames(userName, year, Summary.Plays);

        Summary.BaseGame.Plays = Summary.Plays.Where(play => !play.IsExpansion && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity);
        Summary.BaseGame.Sessions = Summary.Plays.Where(play => !play.IsExpansion).Sum(play => play.Quantity);
        Summary.BaseGame.Unique = Summary.Games.Count(game => !game.IsExpansion);
        Summary.BaseGame.New = Summary.Games.Count(game => !game.IsExpansion && game.IsNew);
        Summary.BaseGame.Highlights = Summary.Games.Count(game => !game.IsExpansion && game.IsHighlight);

        Summary.Expansion.Plays = Summary.Plays.Where(play => play.IsExpansion && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity);
        Summary.Expansion.Sessions = Summary.Plays.Where(play => play.IsExpansion).Sum(play => play.Quantity);
        Summary.Expansion.Unique = Summary.Games.Count(game => game.IsExpansion);
        Summary.Expansion.New = Summary.Games.Count(game => game.IsExpansion && game.IsNew);
        Summary.Expansion.Highlights = Summary.Games.Count(game => game.IsExpansion && game.IsHighlight);

        return Summary;
    }

    private Game[] GetUserGames(string userName, int? year, Play[] plays)
    {
        Logger.LogInformation("Enumerating user collection");
        var userCollection = BggService.GetUserCollectionAsync(userName).ToArrayAsync().GetAwaiter().GetResult();

        Logger.LogInformation("Enumerating user games");
        return plays.GroupBy(play => play.GameId).Select(perGamePlays =>
        {
            var game = new
            {
                Id = perGamePlays.Key,
                Name = perGamePlays.First().GameName,
                perGamePlays.First().IsExpansion,
                ParentId = perGamePlays.First().ParentGameId
            };
            var allGamePlays = GetUserPlays(userName, id: game.Id);

            var gameCollection = userCollection.Where(collection => collection.GameId == game.Id).ToArray();
            if (gameCollection.Length == 0) // not really a problem since we only want comments for metadata
            {
                //Logger.LogWarning("** Failed to find game in user collection:");
                //Logger.LogWarning(game.Name);
                //Logger.LogWarning($"boardgamegeek.com/{(game.IsExpansion ? "boardgameexpansion" : "boardgame")}/{game.Id}");
            }
            else if (gameCollection.Length > 1)
            {
                Logger.LogWarning("** Found multiple games in user collection:");
                Logger.LogWarning(game.Name);
                Logger.LogWarning($"boardgamegeek.com/{(game.IsExpansion ? "boardgameexpansion" : "boardgame")}/{game.Id}");
            }
            var gameComments = string.Join(@"\n", gameCollection.Select(collection => collection.Comments?.Replace("\n", @"\n")));

            return new Game
            {
                Id = game.Id,
                Name = game.Name,
                IsExpansion = game.IsExpansion,
                ParentId = game.ParentId,
                Plays = perGamePlays.Where(play => !play.IsSession || !play.IsIncomplete).Sum(play => play.Quantity),
                Sessions = perGamePlays.Sum(play => play.Quantity),
                CumulativePlays = allGamePlays.Where(play => (year == null || play.Date.Year <= year) && (!play.IsSession || !play.IsIncomplete)).Sum(play => play.Quantity),
                CumulativeSessions = allGamePlays.Where(play => year == null || play.Date.Year <= year).Sum(play => play.Quantity),
                IsHighlight = Game.IsHighlightMatch(gameComments, year),
                IsNew = year != null && allGamePlays.All(play => play.Date.Year >= year),
                Comments = gameComments
            };
        }).ToArray();
    }

    private Play[] GetUserPlays(string userName, int? year = null, int? id = null)
    {
        if (id == null)
        {
            Logger.LogInformation("Enumerating user plays");
        }

        return BggService.GetUserPlaysAsync(userName, year, id).ToArrayAsync().GetAwaiter().GetResult();
    }

    private Play[] ProcessPlays(string userName, Play[] plays)
    {
        Logger.LogInformation("Processing user plays");

        // check plays
        foreach (var play in plays.Where(play => play.IsSession && play.Quantity != 1))
        {
            Logger.LogWarning("** Found multi-play session with unexpected quantity:");
            Logger.LogWarning($"{play.Date:yyyy-MM-dd} {play.GameName}");
            Logger.LogWarning($"boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{play.Date.AddDays(-14):yyyy-MM-dd}/end/{play.Date.AddDays(14):yyyy-MM-dd}");
            Logger.LogWarning($"boardgamegeek.com/{(play.IsExpansion ? "boardgameexpansion" : "boardgame")}/{play.GameId}");
            Logger.LogDebug($">> {play.Quantity}");
            play.Quantity = 1;
            Logger.LogDebug("<< Selected 1");
        }

        // correlate expansion plays with parent plays on same day to determine parent game
        var expansionIds = plays
            .Where(play => play.IsExpansion)
            .Select(play => play.GameId);
        var expansionThings = BggService.GetThingsAsync(expansionIds)
            .ToDictionaryAsync(thing => thing.Id, thing => thing).GetAwaiter().GetResult();

        foreach (var expansionPlay in plays.Where(play => play.IsExpansion))
        {
            var expansion = expansionThings[expansionPlay.GameId];
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
                Logger.LogWarning("** Failed to find parent play for expansion play:");
                Logger.LogWarning($"{expansionPlay.Date:yyyy-MM-dd} {expansionPlay.GameName}");
                Logger.LogWarning($"boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{expansionPlay.Date.AddDays(-14):yyyy-MM-dd}/end/{expansionPlay.Date.AddDays(14):yyyy-MM-dd}");
                Logger.LogWarning($"boardgamegeek.com/boardgameexpansion/{expansionPlay.GameId}");
                if (parentIds.Length > 1)
                {
                    Logger.LogDebug(">> Found multiple linked parent games:");
                    foreach (var parentId in parentIds)
                    {
                        var parent = expansion.Links.Single(link => link.Id == parentId);
                        Logger.LogDebug(parent.Value);
                        Logger.LogDebug($"boardgamegeek.com/boardgame/{parent.Id}");
                    }
                    expansionPlay.ParentGameId = parentIds.First();
                    Logger.LogDebug($"<< Selected {expansionPlay.ParentGameId}");
                }
                else
                {
                    var parent = expansion.Links.Single(link => link.Id == parentIds.Single());
                    Logger.LogDebug(">> Found single parent game:");
                    Logger.LogDebug(parent.Value);
                    Logger.LogDebug($"boardgamegeek.com/boardgame/{parent.Id}");
                    expansionPlay.ParentGameId = parent.Id;
                    Logger.LogDebug($"<< Selected {expansionPlay.ParentGameId}");
                }
            }
            else if (playedParentIds.Length > 1)
            {
                Logger.LogWarning("** Found multiple parent plays for expansion play:");
                Logger.LogWarning($"{expansionPlay.Date:yyyy-MM-dd} {expansionPlay.GameName}");
                Logger.LogWarning($"boardgamegeek.com/plays/bydate/user/{userName}/subtype/boardgame/start/{expansionPlay.Date.AddDays(-14):yyyy-MM-dd}/end/{expansionPlay.Date.AddDays(14):yyyy-MM-dd}");
                Logger.LogWarning($"boardgamegeek.com/boardgameexpansion/{expansionPlay.GameId}");
                Logger.LogDebug(">> Using multiple linked parent games:");
                foreach (var parentId in playedParentIds)
                {
                    var parent = expansion.Links.Single(link => link.Id == parentId);
                    Logger.LogDebug(parent.Value);
                    Logger.LogDebug($"boardgamegeek.com/boardgame/{parent.Id}");
                }
                expansionPlay.ParentGameId = playedParentIds.First();
                Logger.LogDebug($"<< Selected {expansionPlay.ParentGameId}");
            }
            else
            {
                expansionPlay.ParentGameId = playedParentIds.Single();
            }
        }

        return plays;
    }

    public Summary Summary { get; }

    private ILogger Logger { get; }
    private IBggService BggService { get; }
}
