namespace BoardGameGeek.Dungeon;

public sealed class Authenticator
{
    public Authenticator(ILogger<Authenticator> logger, IBggService bggService)
    {
        Logger = logger;
        BggService = bggService;
    }

    public async Task AuthenticateUser(string userName, string? password)
    {
        var fileName = $"BGG-{userName}-Auth.json"; // auth cache
        var options = new JsonSerializerOptions { Converters = { new CookieConverter() }, WriteIndented = true };
        if (password != null)
        {
            Logger.LogInformation("Authenticating user");
            var cookies = await BggService.LoginUserAsync(userName, password);
            var json = JsonSerializer.Serialize(cookies, options);
            await File.WriteAllTextAsync(fileName, json);
        }
        else
        {
            var json = await File.ReadAllTextAsync(fileName);
            var cookies = JsonSerializer.Deserialize<IEnumerable<FlurlCookie>>(json, options)!;
            BggService.LoginUser(cookies);
        }
    }

    private ILogger Logger { get; }
    private IBggService BggService { get; }
}
