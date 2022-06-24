using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.JsonConverters;

public class AccountAddressJsonConverter : JsonConverter<AccountAddress>
{
    public override AccountAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (value == null) throw new JsonException("AccountAddress cannot be null.");
        return AccountAddress.From(value);
    }

    public override void Write(Utf8JsonWriter writer, AccountAddress value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AsString);
    }
}
