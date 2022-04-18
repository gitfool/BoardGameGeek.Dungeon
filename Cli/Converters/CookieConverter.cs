using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoardGameGeek.Dungeon.Extensions;
using Flurl.Http;

namespace BoardGameGeek.Dungeon.Converters;

public sealed class CookieConverter : JsonConverter<FlurlCookie>
{
    public override FlurlCookie Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.CheckStartObject();
        var originUrl = reader.ReadString("OriginUrl");
        var dateReceived = reader.ReadDateTimeOffset("DateReceived");
        var name = reader.ReadString("Name");
        var value = reader.ReadString("Value");
        var expires = reader.ReadNullableDateTimeOffset("Expires");
        var maxAge = reader.ReadNullableInt32("MaxAge");
        var domain = reader.ReadString("Domain");
        var path = reader.ReadString("Path");
        var secure = reader.ReadBoolean("Secure");
        var httpOnly = reader.ReadBoolean("HttpOnly");
        var sameSite = reader.ReadNullableEnum<SameSite>("SameSite");
        reader.ReadEndObject();

        return new FlurlCookie(name, value, originUrl, dateReceived)
        {
            Expires = expires,
            MaxAge = maxAge,
            Domain = domain,
            Path = path,
            Secure = secure,
            HttpOnly = httpOnly,
            SameSite = sameSite
        };
    }

    public override void Write(Utf8JsonWriter writer, FlurlCookie value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("OriginUrl", value.OriginUrl);
        writer.WriteDateTimeOffset("DateReceived", value.DateReceived);
        writer.WriteString("Name", value.Name);
        writer.WriteString("Value", value.Value);
        writer.WriteNullableDateTimeOffset("Expires", value.Expires);
        writer.WriteNullableNumber("MaxAge", value.MaxAge);
        writer.WriteString("Domain", value.Domain);
        writer.WriteString("Path", value.Path);
        writer.WriteBoolean("Secure", value.Secure);
        writer.WriteBoolean("HttpOnly", value.HttpOnly);
        writer.WriteNullableEnum("SameSite", value.SameSite);
        writer.WriteEndObject();
    }
}
