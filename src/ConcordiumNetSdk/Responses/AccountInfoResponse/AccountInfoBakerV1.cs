using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account baker version 1.
/// </summary>
public record AccountInfoBakerV1 : AccountInfo
{
    /// <summary>
    /// Gets or initiates the account baker details version 1.
    /// </summary>
    [JsonPropertyName("accountBaker")]
    public AccountBakerDetailsV1 Baker { get; init; }
}
