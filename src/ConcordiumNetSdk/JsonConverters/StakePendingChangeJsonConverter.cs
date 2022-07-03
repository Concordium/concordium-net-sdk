using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.AccountInfoResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class StakePendingChangeJsonConverter : JsonConverter<StakePendingChange>
{
    public override StakePendingChange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);

        JsonElement change;
        
        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "epoch", out _) && 
            JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "change", out change))
        {
            string? changeValue = change.GetString();
            if (string.IsNullOrEmpty(changeValue)) throw new JsonException("The change value can not be null or empty.");

            if (changeValue == "ReduceStake")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(ReduceStakePendingChangeV0), options) as StakePendingChange;
            }

            if (changeValue == "RemoveBaker")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(RemovalPendingChangeV0), options) as StakePendingChange;
            }

            throw new JsonException($"The change value can not be '{changeValue}'.");
        }

        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "effectiveTime", out _) &&
            JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "change", out change))
        {
            string? changeValue = change.GetString();
            if (string.IsNullOrEmpty(changeValue)) throw new JsonException("The change value can not be null or empty.");

            if (changeValue == "ReduceStake")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(ReduceStakePendingChangeV1), options) as StakePendingChange;
            }

            if (changeValue == "RemoveStake")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(RemovalPendingChangeV1), options) as StakePendingChange;
            }

            throw new JsonException($"The change value can not be '{changeValue}'.");
        }

        throw new JsonException($"Incorrect json data for type {nameof(StakePendingChange)}.");
    }

    public override void Write(Utf8JsonWriter writer, StakePendingChange value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
