using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Services;

namespace BoardGameGeek.Dungeon.CommandLine
{
    public static class Bootstrap
    {
        static Bootstrap()
        {
            var userNameArgument = new Argument<string> { Name = "username", Description = "Geek username." };
            var passwordArgument = new Argument<string> { Name = "password", Description = "Geek password." };

            var passwordOption = new Option<string>(new[] { "--password", "-p" }, "Geek password. Defaults to last specified.");

            var dateOption = new Option<DateTime>(new[] { "--date", "-d" }, () => DateTime.Now.Date, "Date of play. Defaults to current date.").WithFormat("yyyy-MM-dd");
            var locationOption = new Option<string>(new[] { "--location", "-l" }, "Location of play. Defaults to unspecified.");
            var quantityOption = new Option<int>(new[] { "--quantity", "-q" }, () => 1, "Quantity of play. Defaults to 1.");
            var gameIdOption = new Option<int?>(new[] { "--game-id", "-g" }, "Game id of play. Defaults to unspecified.");
            var gameNameOption = new Option<string>(new[] { "--game-name" }, "Game name of play. Defaults to unspecified.");
            var lengthOption = new Option<int>(new[] { "--length" }, "Length of play (minutes). Defaults to 0.");
            var incompleteOption = new Option<bool>(new[] { "--incomplete" }, "Incomplete play. Defaults to false.");
            var noWinStatsOption = new Option<bool>(new[] { "--no-win-stats" }, "No win stats for play. Defaults to false.");
            var commentsOption = new Option<string>(new[] { "--comments" }, "Comments for play. Defaults to unspecified.");

            var allOption = new Option<bool>(new[] { "--all", "-a" }, "Analyze all override. Defaults to false.");
            var yearOption = new Option<int?>(new[] { "--year", "-y" }, () => DateTime.Now.Year, "Year to analyze. Defaults to current year.");

            var loginUserCommand = new Command("login", "Login user.") { userNameArgument, passwordArgument };
            loginUserCommand.Handler = CommandHandler.Create<string, string>(LoginUserAsync);
            var logPlayCommand = new Command("play", "Log user play.") { userNameArgument, passwordOption, dateOption, locationOption, quantityOption, gameIdOption, gameNameOption, lengthOption, incompleteOption, noWinStatsOption, commentsOption };
            logPlayCommand.AddValidator(symbol => !(symbol.Children.Contains("game-id") ^ symbol.Children.Contains("game-name")) ? "Play must specify one of game id or game name" : null);
            logPlayCommand.Handler = HandlerDescriptor.FromDelegate(new Func<string, string, DateTime, string, int, int?, string, int, bool, bool, string, Task>(LogUserPlayAsync)).GetCommandHandler();
            var getUserPlaysCommand = new Command("plays", "Get user plays.") { userNameArgument, allOption, yearOption };
            getUserPlaysCommand.Handler = CommandHandler.Create<string, bool, int?>(GetUserPlaysAsync);
            var getUserStatsCommand = new Command("stats", "Get user stats.") { userNameArgument, allOption, yearOption };
            getUserStatsCommand.Handler = CommandHandler.Create<string, bool, int?>(GetUserStatsAsync);

            Parser = new CommandLineBuilder(new RootCommand("A command line tool for interacting with the BoardGameGeek API"))
                .AddCommand(loginUserCommand)
                .AddCommand(new Command("log", "Log commands.") { logPlayCommand })
                .AddCommand(new Command("get", "Get commands.") { getUserPlaysCommand, getUserStatsCommand })
                .UseDefaults()
                .Build();
        }

        private static async Task GetUserPlaysAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderPlays(userName, year, processor.ProcessPlays(userName, year));
        }

        private static async Task GetUserStatsAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderStats(userName, year, processor.ProcessStats(userName, year));
        }

        private static Task LoginUserAsync(string userName, string password)
        {
            var authenticator = new Authenticator(new BggService());
            return authenticator.AuthenticateUser(userName, password);
        }

        private static async Task LogUserPlayAsync(string userName, string password, DateTime date, string location, int quantity, int? gameId, string gameName, int length, bool incomplete, bool noWinStats, string comments)
        {
            if (gameId == null)
            {
                gameId = 1; //TODO search for game name
            }
            var bggService = new BggService();
            var authenticator = new Authenticator(bggService);
            await authenticator.AuthenticateUser(userName, password);
            var logger = new Logger(bggService);
            await logger.LogPlay(date, location, quantity, gameId.Value, length, incomplete, noWinStats, comments);
        }

        public static Parser Parser { get; }
    }
}
