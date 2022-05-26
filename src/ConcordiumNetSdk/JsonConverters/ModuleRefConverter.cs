using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.JsonConverters;

public class ModuleRefConverter : JsonConverter<ModuleRef>
{
    public override ModuleRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (value == null) throw new JsonException("ModuleRef cannot be null.");
        return ModuleRef.From(value);
    }

    public override void Write(Utf8JsonWriter writer, ModuleRef value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AsString);
    }
}
