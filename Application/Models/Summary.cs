using System;

namespace BoardGameGeek.Dungeon.Models
{
    public class Summary
    {
        public Summary()
        {
            Plays = Array.Empty<Play>();
            Games = Array.Empty<Game>();
            BaseGame = new Stats();
            Expansion = new Stats();
        }

        public Play[] Plays { get; set; }
        public Game[] Games { get; set; }

        public Stats BaseGame { get; set; }
        public Stats Expansion { get; set; }
    }
}
