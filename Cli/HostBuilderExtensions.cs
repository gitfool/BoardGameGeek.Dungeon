using Serilog;

namespace BoardGameGeek.Dungeon;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder hostBuilder, string[] args) =>
        hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            ((List<IConfigurationSource>)builder.Sources).RemoveAll(
                source => source.GetType() == typeof(EnvironmentVariablesConfigurationSource) || source.GetType() == typeof(CommandLineConfigurationSource));

            var appRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
            builder.AddYamlFile(Path.Combine(appRoot, "config", "_default.yaml"), false, false) // global defaults
                .AddYamlFile(Path.Combine(appRoot, "config", $"_{context.HostingEnvironment.EnvironmentName.ToLowerInvariant()}.yaml"), true, false); // dotnet environment; development, production

            builder.AddEnvironmentVariables() // env vars
                .AddCommandLine(args); // cli
        });

    public static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices((context, services) =>
        {
            services.Configure<Config>(context.Configuration.GetSection(Constants.ConfigKey));

            services.AddSingleton<IBggService, BggService>();
            services.AddTransient<Authenticator>();
            services.AddTransient<Processor>();
            services.AddTransient<Recorder>();
            services.AddTransient<Renderer>();
        });

    public static async Task<int> RunCommandAsync(this IHostBuilder hostBuilder, string[] args)
    {
        try
        {
            var commandApp = new CommandApp(new HostTypeRegistrar(hostBuilder));
            commandApp.Configure(config =>
            {
                config.SetApplicationName(Constants.AppName);
                config.UseAssemblyInformationalVersion();
                config.AddCommand<ConfigCommand>("config");
                config.AddCommand<LoginCommand>("login");
                config.AddBranch("get", branch =>
                {
                    branch.AddCommand<GetPlaysCommand>("plays");
                    branch.AddCommand<GetStatsCommand>("stats");
                });
                config.AddBranch("log", branch => branch.AddCommand<LogPlayCommand>("play"));
                //config.PropagateExceptions();
            });
            return await commandApp.RunAsync(args);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths);
            return -1;
        }
    }

    public static IHostBuilder UseSerilog(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.When(_ => context.HostingEnvironment.IsDevelopment(), enrich => enrich.With<SourceContextUqnEnricher>())
                .Enrich.WithProperty("ApplicationName", Constants.AppName);

            if (context.HostingEnvironment.IsDevelopment())
            {
                var writeToConsole = context.Configuration.GetSection("Serilog:WriteTo").GetChildren()
                    .Any(section => section.GetValue<string>("Name") == "Console");

                if (!writeToConsole)
                {
                    config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceContextUqn}) {Message:lj}{NewLine}{Exception}");
                }
            }
            else
            {
                config.WriteTo.Console();
            }
        }, true);
}
