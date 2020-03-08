using System;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Pocket;

namespace BoardGameGeek.Dungeon
{
    public class Program
    {
        private static Task<int> Main(string[] args)
        {
            LogEvents.Subscribe(entry =>
            {
                var (message, _) = entry.Evaluate();
                Console.WriteLine($"{entry.TimestampUtc.ToLocalTime():HH:mm:ss} {message}");
            });

            return CommandLine.Bootstrap.Parser.InvokeAsync(args);
        }
    }
}
