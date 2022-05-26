using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.JsonConverters;

public class BlockHashConverter : JsonConverter<BlockHash>
{
    public override BlockHash Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (value == null) throw new JsonException("BlockHash cannot be null.");
        return BlockHash.From(value);
    }

    public override void Write(Utf8JsonWriter writer, BlockHash value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AsString);
    }
}
