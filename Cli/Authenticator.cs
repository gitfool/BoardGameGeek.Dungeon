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
        var fileName = $"BGG-{userName}-Auth.txt"; // auth cache
        if (password != null)
        {
            Logger.LogInformation("Authenticating user");
            var cookies = await BggService.LoginUserAsync(userName, password);
            await using var writer = File.CreateText(fileName);
            cookies.WriteTo(writer);
        }
        else
        {
            var reader = File.OpenText(fileName);
            var cookies = CookieJar.LoadFrom(reader);
            BggService.LoginUser(cookies);
        }
    }

    private ILogger Logger { get; }
    private IBggService BggService { get; }
}
