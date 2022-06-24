using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.BlockSummaryResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class BlockSummaryJsonConverter : JsonConverter<BlockSummary>
{
    public override BlockSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);

        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "protocolVersion", out _))
        {
            return JsonSerializer.Deserialize(ref reader, typeof(BlockSummaryV1), options) as BlockSummary;
        }

        return JsonSerializer.Deserialize(ref reader, typeof(BlockSummaryV0), options) as BlockSummary;
    }

    public override void Write(Utf8JsonWriter writer, BlockSummary value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
