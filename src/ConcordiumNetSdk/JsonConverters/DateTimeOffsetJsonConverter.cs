using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.JsonConverters;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    const string TzDateFormat = "O";

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.UtcDateTime.ToString(TzDateFormat, CultureInfo.InvariantCulture));
    

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                string? value = reader.GetString();
                if (value == null) throw new JsonException("DateTimeOffset value cannot be null.");
                return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            default:
                throw new JsonException("Invalid JsonTokenType for DateTimeOffset value.");
        }
    }
}
