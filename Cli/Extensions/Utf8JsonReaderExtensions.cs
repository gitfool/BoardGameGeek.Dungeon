namespace BoardGameGeek.Dungeon.Extensions;

public static class Utf8JsonReaderExtensions
{
    public static void CheckStartObject(this ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
    }

    public static void CheckEndObject(this ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException();
        }
    }

    public static T GetEnum<T>(this ref Utf8JsonReader reader) where T : struct, Enum => Enum.Parse<T>(reader.GetString()!);

    public static DateTime GetDateTime(this ref Utf8JsonReader reader) => DateTime.Parse(reader.GetString()!);

    public static DateTimeOffset GetDateTimeOffset(this ref Utf8JsonReader reader) => DateTimeOffset.Parse(reader.GetString()!);

    public static TimeSpan GetTimeSpan(this ref Utf8JsonReader reader) => TimeSpan.Parse(reader.GetString()!, CultureInfo.InvariantCulture);

    public static bool? GetNullableBoolean(this ref Utf8JsonReader reader) => reader.TokenType != JsonTokenType.Null ? reader.GetBoolean() : null;

    public static int? GetNullableInt32(this ref Utf8JsonReader reader) => reader.TokenType != JsonTokenType.Null ? reader.GetInt32() : null;

    public static T? GetNullableEnum<T>(this ref Utf8JsonReader reader) where T : struct, Enum => reader.TokenType != JsonTokenType.Null ? reader.GetEnum<T>() : null;

    public static DateTime? GetNullableDateTime(this ref Utf8JsonReader reader) => reader.TokenType != JsonTokenType.Null ? reader.GetDateTime() : null;

    public static DateTimeOffset? GetNullableDateTimeOffset(this ref Utf8JsonReader reader) => reader.TokenType != JsonTokenType.Null ? reader.GetDateTimeOffset() : null;

    public static TimeSpan? GetNullableTimeSpan(this ref Utf8JsonReader reader) => reader.TokenType != JsonTokenType.Null ? reader.GetTimeSpan() : null;

    public static void ReadStartObject(this ref Utf8JsonReader reader)
    {
        reader.Read();
        reader.CheckStartObject();
    }

    public static void ReadEndObject(this ref Utf8JsonReader reader)
    {
        reader.Read();
        reader.CheckEndObject();
    }

    public static bool ReadBoolean(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetBoolean();

    public static string? ReadString(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetString();

    public static T ReadEnum<T>(this ref Utf8JsonReader reader, string propertyName) where T : struct, Enum => reader.ReadProperty(propertyName).GetEnum<T>();

    public static DateTime ReadDateTime(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetDateTime();

    public static DateTimeOffset ReadDateTimeOffset(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetDateTimeOffset();

    public static TimeSpan ReadTimeSpan(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetTimeSpan();

    public static bool? ReadNullableBoolean(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetNullableBoolean();

    public static int? ReadNullableInt32(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetNullableInt32();

    public static T? ReadNullableEnum<T>(this ref Utf8JsonReader reader, string propertyName) where T : struct, Enum => reader.ReadProperty(propertyName).GetNullableEnum<T>();

    public static DateTime? ReadNullableDateTime(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetNullableDateTime();

    public static DateTimeOffset? ReadNullableDateTimeOffset(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetNullableDateTimeOffset();

    public static TimeSpan? ReadNullableTimeSpan(this ref Utf8JsonReader reader, string propertyName) => reader.ReadProperty(propertyName).GetNullableTimeSpan();

    private static ref Utf8JsonReader ReadProperty(this ref Utf8JsonReader reader, string propertyName)
    {
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != propertyName)
        {
            throw new JsonException();
        }
        reader.Read();
        return ref reader;
    }
}
