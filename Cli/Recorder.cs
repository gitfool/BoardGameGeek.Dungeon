using System;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Models;
using BoardGameGeek.Dungeon.Services;
using Microsoft.Extensions.Logging;

namespace BoardGameGeek.Dungeon
{
    public sealed class Recorder
    {
        public Recorder(ILogger<Recorder> logger, IBggService bggService)
        {
            Logger = logger;
            BggService = bggService;
        }

        public Task LogPlay(DateTime date, string? location, int quantity, int gameId, int length, bool isIncomplete, bool noWinStats, string? comments)
        {
            var play = new Play
            {
                Date = date,
                Location = location,
                Quantity = Math.Max(1, quantity),
                GameId = gameId,
                Length = Math.Max(0, length),
                IsIncomplete = isIncomplete,
                NoWinStats = noWinStats,
                Comments = comments
            };
            Logger.LogInformation($"Logging play {play}");
            return BggService.LogUserPlayAsync(play);
        }

        private ILogger Logger { get; }
        private IBggService BggService { get; }
    }
}
