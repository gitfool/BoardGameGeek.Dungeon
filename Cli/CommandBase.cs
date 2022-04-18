namespace BoardGameGeek.Dungeon;

public abstract class CommandBase<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected CommandBase(IOptions<Config> options, ILogger logger)
    {
        Config = options.Value;
        Logger = logger;
    }

    // ReSharper disable once RedundantNullableFlowAttribute
    public override int Execute([NotNull] CommandContext context, [NotNull] TSettings settings)
    {
        try
        {
            return OnExecute(context, settings);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths);
            return -1;
        }
    }

    protected abstract int OnExecute(CommandContext context, TSettings settings);

    protected Config Config { get; }
    protected ILogger Logger { get; }
}
