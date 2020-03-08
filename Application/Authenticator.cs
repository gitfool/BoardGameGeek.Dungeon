using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            if (password != null)
            {
                Log.Info("Authenticating user");
                var cookies = await BggService.LoginUserAsync(userName, password);
                var json = JsonConvert.SerializeObject(cookies, new JsonSerializerSettings
                {
                    ContractResolver = new CookieContractResolver(),
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    Formatting = Formatting.Indented
                });
                await File.WriteAllTextAsync(fileName, json);
            }
            else
            {
                var cookies = JsonConvert.DeserializeObject<IDictionary<string, Cookie>>(await File.ReadAllTextAsync(fileName));
                BggService.LoginUser(cookies);
            }
        }

        private class CookieContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) => // filter properties so cookies roundtrip serialization
                base.CreateProperties(type, memberSerialization).Where(property => property.PropertyName != "Comment" && property.PropertyName != "Port").ToList();
        }

        private IBggService BggService { get; }
    }
}
