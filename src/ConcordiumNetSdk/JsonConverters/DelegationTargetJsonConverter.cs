using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.AccountInfoResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class DelegationTargetJsonConverter : JsonConverter<DelegationTarget>
{
    public override DelegationTarget? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);
        
        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "delegateType", out JsonElement delegateType))
        {
            string? delegateTypeValue = delegateType.GetString();
            if (string.IsNullOrEmpty(delegateTypeValue)) throw new JsonException($"The {nameof(delegateType)} value can not be null or empty.");

            if (delegateTypeValue == "Baker")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(DelegationTargetBaker), options) as DelegationTarget;
            }

            if (delegateTypeValue == "Passive")
            {
                return JsonSerializer.Deserialize(ref reader, typeof(DelegationTargetPassiveDelegation), options) as DelegationTarget;
            }

            throw new JsonException($"The change value can not be '{delegateTypeValue}'.");
        }

        throw new JsonException($"Incorrect json data for type {nameof(DelegationTarget)}.");
    }

    public override void Write(Utf8JsonWriter writer, DelegationTarget value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
