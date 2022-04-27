using System.Text.Json;

namespace ConcordiumNetSdk.JsonConverters;

public static class JsonConverterHelper
{
    public static void EnsureTokenType(Utf8JsonReader reader, JsonTokenType tokenType)
    {
        if (reader.TokenType != tokenType) throw new JsonException($"Must be {tokenType}.");
    }
}
