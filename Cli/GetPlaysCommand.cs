using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace BoardGameGeek.Dungeon;

public sealed partial class GetPlaysCommand : AsyncCommandBase<GetPlaysCommand.Settings>
{
    public GetPlaysCommand(IOptions<Config> options, ILogger<GetPlaysCommand> logger, Processor processor, Renderer renderer)
        : base(options, logger)
    {
        Processor = processor;
        Renderer = renderer;
    }

    protected override async Task<int> OnExecuteAsync(CommandContext context, Settings settings)
    {
        var year = !settings.All ? settings.Year : null;
        await Renderer.RenderPlays(settings.UserName, year, Processor.ProcessPlays(settings.UserName, year));
        return 0;
    }

    private Processor Processor { get; }
    private Renderer Renderer { get; }
}
