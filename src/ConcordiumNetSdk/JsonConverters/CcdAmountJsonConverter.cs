using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.JsonConverters;

public class CcdAmountJsonConverter : JsonConverter<CcdAmount>
{
    public override CcdAmount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (value == null) throw new JsonException("Amount cannot be null.");
        ulong microCcd = ulong.Parse(value);
        return CcdAmount.FromMicroCcd(microCcd);
    }

    public override void Write(Utf8JsonWriter writer, CcdAmount value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FormattedMicroCcd);
    }
}
