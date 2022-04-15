using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BoardGameGeek.Dungeon.Extensions
{
    public static class Utf8JsonWriterExtensions
    {
        public static void WriteEnum<T>(this Utf8JsonWriter writer, string propertyName, T value) where T : struct, Enum => writer.WriteString(propertyName, value.ToString());

        public static void WriteDateTime(this Utf8JsonWriter writer, string propertyName, DateTime value) => writer.WriteString(propertyName, value);

        public static void WriteDateTimeOffset(this Utf8JsonWriter writer, string propertyName, DateTimeOffset value) => writer.WriteString(propertyName, value);

        public static void WriteTimeSpan(this Utf8JsonWriter writer, string propertyName, TimeSpan value) => writer.WriteString(propertyName, value.ToString(null, CultureInfo.InvariantCulture));

        public static void WriteNullableBoolean(this Utf8JsonWriter writer, string propertyName, bool? value) => writer.WriteNull(propertyName, value)?.WriteBoolean(propertyName, value!.Value);

        public static void WriteNullableNumber(this Utf8JsonWriter writer, string propertyName, int? value) => writer.WriteNull(propertyName, value)?.WriteNumber(propertyName, value!.Value);

        public static void WriteNullableEnum<T>(this Utf8JsonWriter writer, string propertyName, T? value) where T : struct, Enum => writer.WriteNull(propertyName, value)?.WriteEnum(propertyName, value!.Value);

        public static void WriteNullableDateTime(this Utf8JsonWriter writer, string propertyName, DateTime? value) => writer.WriteNull(propertyName, value)?.WriteDateTime(propertyName, value!.Value);

        public static void WriteNullableDateTimeOffset(this Utf8JsonWriter writer, string propertyName, DateTimeOffset? value) => writer.WriteNull(propertyName, value)?.WriteDateTimeOffset(propertyName, value!.Value);

        public static void WriteNullableTimeSpan(this Utf8JsonWriter writer, string propertyName, TimeSpan? value) => writer.WriteNull(propertyName, value)?.WriteTimeSpan(propertyName, value!.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Utf8JsonWriter WriteNull<T>(this Utf8JsonWriter writer, string propertyName, T? value) where T : struct
        {
            if (!value.HasValue)
            {
                writer.WriteNull(propertyName);
                return null;
            }
            return writer;
        }
    }
}
