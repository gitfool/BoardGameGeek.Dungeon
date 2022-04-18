namespace BoardGameGeek.Dungeon;

public sealed partial class LoginCommand : AsyncCommandBase<LoginCommand.Settings>
{
    public LoginCommand(IOptions<Config> options, ILogger<LoginCommand> logger, Authenticator authenticator)
        : base(options, logger)
    {
        Authenticator = authenticator;
    }

    protected override async Task<int> OnExecuteAsync(CommandContext context, Settings settings)
    {
        await Authenticator.AuthenticateUser(settings.UserName, settings.Password);
        return 0;
    }

    private Authenticator Authenticator { get; }
}
