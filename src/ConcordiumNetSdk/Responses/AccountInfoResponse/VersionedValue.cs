using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: implement as value type with all methods maybe some where on T
/// <summary>
/// Represents the versioned values.
/// </summary>
/// <typeparam name="T">type ov value to be versioned.</typeparam>
public record VersionedValue<T>
{
    /// <summary>
    /// Gets or initiates the version.
    /// </summary>
    [JsonPropertyName("v")]
    public int Version { get; init; }

    /// <summary>
    /// Gets or initiates the value.
    /// </summary>
    public T Value { get; init; }
}
