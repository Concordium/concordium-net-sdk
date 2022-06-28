using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

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
