using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a verify key.
/// </summary>
public record VerifyKey
{
    /// <summary>
    /// Gets or initiates the scheme id.
    /// </summary>
    public string SchemeId { get; init; }

    /// <summary>
    /// Gets or initiates the verify key.
    /// </summary>
    [JsonPropertyName("verifyKey")]
    public string VerifyKeyValue { get; init; }
}
