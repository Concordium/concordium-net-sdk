using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.JsonConverters;

public class NonceConverter : JsonConverter<Nonce>
{
    public override Nonce Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetUInt64();
        return new Nonce(value);
    }

    public override void Write(Utf8JsonWriter writer, Nonce value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.AsUInt64);
    }
}
