using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.AccountInfoResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class AccountInfoJsonConverter : JsonConverter<AccountInfo>
{
    public override AccountInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);

        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "accountDelegation", out _))
        {
            return JsonSerializer.Deserialize(ref reader, typeof(AccountInfoDelegator), options) as AccountInfo;
        }

        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "accountBaker.bakerPoolInfo", out _))
        {
            return JsonSerializer.Deserialize(ref reader, typeof(AccountInfoBakerV1), options) as AccountInfo;
        }

        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "accountBaker", out _))
        {
            return JsonSerializer.Deserialize(ref reader, typeof(AccountInfoBakerV0), options) as AccountInfo;
        }

        return JsonSerializer.Deserialize(ref reader, typeof(AccountInfoSimple), options) as AccountInfo;
    }

    public override void Write(Utf8JsonWriter writer, AccountInfo value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
