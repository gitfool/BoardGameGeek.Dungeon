namespace BoardGameGeek.Dungeon;

public sealed partial class LogPlayCommand : AsyncCommandBase<LogPlayCommand.Settings>
{
    public LogPlayCommand(IOptions<Config> options, ILogger<LogPlayCommand> logger, Authenticator authenticator, Recorder recorder)
        : base(options, logger)
    {
        Authenticator = authenticator;
        Recorder = recorder;
    }

    protected override async Task<int> OnExecuteAsync(CommandContext context, Settings settings)
    {
        await Authenticator.AuthenticateUser(settings.UserName, settings.Password);
        var gameId = settings.GameId ?? 1; //TODO search for game name
        await Recorder.LogPlay(settings.Date, settings.Location, settings.Quantity, gameId, settings.Length, settings.Incomplete, settings.NoWinStats, settings.Comments);
        return 0;
    }

    private Authenticator Authenticator { get; }
    private Recorder Recorder { get; }
}
