using System;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Models;
using BoardGameGeek.Dungeon.Services;
using Pocket;
using static Pocket.Logger<BoardGameGeek.Dungeon.Logger>;

namespace BoardGameGeek.Dungeon
{
    public class Logger
    {
        public Logger(IBggService bggService)
        {
            BggService = bggService;
        }

        public Task LogPlay(DateTime date, string location, int quantity, int gameId, int length, bool isIncomplete, bool noWinStats, string comments)
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
            Log.Info($"Logging play {play}");
            return BggService.LogUserPlayAsync(play);
        }

        private IBggService BggService { get; }
    }
}
