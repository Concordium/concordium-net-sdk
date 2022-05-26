using System.Text.Json;
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
                new NonceConverter(),
                new BlockHashConverter(),
                new DateTimeOffsetConverter(),
                new AccountAddressConverter(),
                new ModuleRefConverter(),
                new ContractAddressConverter(),
                new CcdAmountConverter(),
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
