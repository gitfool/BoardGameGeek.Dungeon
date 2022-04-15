using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Converters;
using BoardGameGeek.Dungeon.Services;
using Flurl.Http;
using Pocket;
using static Pocket.Logger<BoardGameGeek.Dungeon.Authenticator>;

namespace BoardGameGeek.Dungeon
{
    public class Authenticator
    {
        public Authenticator(IBggService bggService)
        {
            BggService = bggService;
        }

        public async Task AuthenticateUser(string userName, string password)
        {
            var fileName = $"BGG-{userName}-Auth.json"; // auth cache
            var options = new JsonSerializerOptions { Converters = { new CookieConverter() }, WriteIndented = true };
            if (password != null)
            {
                Log.Info("Authenticating user");
                var cookies = await BggService.LoginUserAsync(userName, password);
                var json = JsonSerializer.Serialize(cookies, options);
                await File.WriteAllTextAsync(fileName, json);
            }
            else
            {
                var json = await File.ReadAllTextAsync(fileName);
                var cookies = JsonSerializer.Deserialize<IEnumerable<FlurlCookie>>(json, options);
                BggService.LoginUser(cookies);
            }
        }

        private IBggService BggService { get; }
    }
}
