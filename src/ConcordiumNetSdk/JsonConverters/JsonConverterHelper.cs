using System.Text.Json;

namespace ConcordiumNetSdk.JsonConverters;

public static class JsonConverterHelper
{
    public static void EnsureTokenType(Utf8JsonReader reader, JsonTokenType tokenType)
    {
        if (reader.TokenType != tokenType) throw new JsonException($"Must be {tokenType}.");
    }

    public static JsonElement GetJsonElement(JsonElement jsonElement, string path)
    {
        if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined) return default;

        string[] segments = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            jsonElement = jsonElement.TryGetProperty(segment, out JsonElement value) ? value : default;
            if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined) return default;
        }

        return jsonElement;
    }

    public static bool TryGetJsonElement(JsonElement jsonElement, string path, out JsonElement expectedJsonElement)
    {
        if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            expectedJsonElement = default;
            return false;
        }

        string[] segments = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            jsonElement = jsonElement.TryGetProperty(segment, out JsonElement value) ? value : default;
            if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                expectedJsonElement = default;
                return false;
            }
        }

        expectedJsonElement = jsonElement;
        return true;
    }
}
