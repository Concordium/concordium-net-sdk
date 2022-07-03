using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.JsonConverters;

namespace ConcordiumNetSdk;

public static class CustomJsonSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions;

    static CustomJsonSerializer()
    {
        JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new AccountCredentialJsonConverter(),
                new NonceJsonConverter(),
                new BlockHashJsonConverter(),
                new DateTimeOffsetJsonConverter(),
                new AccountAddressJsonConverter(),
                new ModuleRefJsonConverter(),
                new ContractAddressJsonConverter(),
                new CcdAmountJsonConverter(),
                new JsonStringEnumMemberConverter(),
                new TransactionSummaryJsonConverter(),
                new EventResultJsonConverter(),
                new EventJsonConverter(),
                new BlockSummaryJsonConverter(),
                new AccountInfoJsonConverter(),
                new StakePendingChangeJsonConverter(),
                new DelegationTargetJsonConverter(),
            }   
        };
    }

    public static string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, JsonSerializerOptions);
    }

    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
    }
}
