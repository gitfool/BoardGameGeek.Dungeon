using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BoardGameGeek.Dungeon.Services;
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
                var json = JsonSerializer.Serialize(cookies, new JsonSerializerOptions
                {
                    Converters = { new CookieConverter() },
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(fileName, json);
            }
            else
            {
                var json = await File.ReadAllTextAsync(fileName);
                var cookies = JsonSerializer.Deserialize<IDictionary<string, Cookie>>(json);
                BggService.LoginUser(cookies);
            }
        }

        private class CookieConverter : JsonConverter<Cookie>
        {
            public override Cookie Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

            public override void Write(Utf8JsonWriter writer, Cookie value, JsonSerializerOptions options)
            {
                // select minimal properties for roundtrip
                writer.WriteStartObject();
                writer.WriteString("Domain", value.Domain);
                writer.WriteString("Expires", value.Expires);
                writer.WriteString("Name", value.Name);
                writer.WriteString("Path", value.Path);
                writer.WriteString("TimeStamp", value.TimeStamp);
                writer.WriteString("Value", value.Value);
                writer.WriteEndObject();
            }
        }

        private IBggService BggService { get; }
    }
}
