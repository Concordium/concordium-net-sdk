using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.TransactionStatusResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class TransactionSummaryJsonConverter : JsonConverter<TransactionSummary>
{
    private static readonly Dictionary<string, Type> TypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // the key is discriminator value
        {"transferWithMemo", typeof(TransferWithMemoTransactionSummary)}
    };

    public override TransactionSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);

        JsonElement contents = JsonConverterHelper.GetJsonElement(jsonDocument.RootElement, "type.contents");
        string? contentsValue = contents.GetString();

        if (string.IsNullOrEmpty(contentsValue)) 
            throw new JsonException("The type.contents value can not be null or empty.");

        if (!TypeMap.TryGetValue(contentsValue, out Type? targetType))
            targetType = typeof(GenericTransactionSummary);

        return JsonSerializer.Deserialize(ref reader, targetType, options) as TransactionSummary;
    }

    public override void Write(Utf8JsonWriter writer, TransactionSummary value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
