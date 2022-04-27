using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.AccountInfoResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class AccountCredentialJsonConverter : JsonConverter<AccountCredential>
{
    private static readonly Dictionary<string, Type> TypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        {"initial", typeof(InitialAccountCredential)},
        {"normal", typeof(NormalAccountCredential)}
    };

    public override AccountCredential? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        var readerClone = reader;
        
        using var jsonDocument = JsonDocument.ParseValue(ref readerClone);
        var jsonObject = jsonDocument.RootElement;
        var typeProperty = jsonObject.GetProperty("type");
        var typeValue = typeProperty.GetString();

        if (string.IsNullOrEmpty(typeValue) || !TypeMap.TryGetValue(typeValue, out var targetType))
            throw new JsonException($"Type value: '{typeValue ?? "null"}' is not supported.");

        return JsonSerializer.Deserialize(ref reader, targetType, options) as AccountCredential;
    }

    public override void Write(Utf8JsonWriter writer, AccountCredential value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
