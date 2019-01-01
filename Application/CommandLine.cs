using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace BoardGameGeek.Dungeon
{
    public class CommandLine
    {
        static CommandLine()
        {
            // arguments
            var userNameArgument = new Argument<string> { Name = "username", Description = "Geek user name." };

            // options
            var allOption = new Option(new[] { "-a", "--all" }, "Analyze all override.", new Argument<bool>());
            var yearOption = new Option(new[] { "-y", "--year" }, "Year to analyze. Defaults to current year.", new Argument<int>(DateTime.Now.Year));

            // parser
            Parser = new CommandLineBuilder(new RootCommand("A command line tool for interacting with the BoardGameGeek API")
            {
                new Command("plays", "Get user plays.", new[] { allOption, yearOption }, userNameArgument,
                    handler: CommandHandler.Create(typeof(CommandLine).GetMethod(nameof(PlaysAsync)))),
                new Command("stats", "Get user stats.", new[] { allOption, yearOption }, userNameArgument,
                    handler: CommandHandler.Create(typeof(CommandLine).GetMethod(nameof(StatsAsync))))
            })
                .UseDefaults()
                .Build();
        }

        // commands
        public static async Task PlaysAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderPlays(userName, year, await processor.ProcessPlays(userName, year));
        }

        public static async Task StatsAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderStats(userName, year, await processor.ProcessStats(userName, year));
        }

        public static Parser Parser { get; }
    }
}
