using System;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.CommandLine;
using Pocket;

namespace BoardGameGeek.Dungeon
{
    public static class Program
    {
        private static Task<int> Main(string[] args)
        {
            LogEvents.Subscribe(entry =>
            {
                var (message, _) = entry.Evaluate();
                Console.WriteLine($"{entry.TimestampUtc.ToLocalTime():HH:mm:ss} {message}");
            });

            return Bootstrap.Parser.InvokeAsync(args);
        }
    }
}
