using System;

namespace BoardGameGeek.Dungeon
{
    public class Summary
    {
        public Summary()
        {
            UserPlays = Array.Empty<PlayDto>();
            UserGames = Array.Empty<GameDto>();
            BaseGame = new Stats();
            Expansion = new Stats();
        }

        public PlayDto[] UserPlays { get; set; }
        public GameDto[] UserGames { get; set; }

        public Stats BaseGame { get; set; }
        public Stats Expansion { get; set; }
    }
}
