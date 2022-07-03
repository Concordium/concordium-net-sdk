using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account baker version 0.
/// </summary>
public record AccountInfoBakerV0 : AccountInfo
{
    /// <summary>
    /// Gets or initiates the account baker details version 0.
    /// </summary>
    [JsonPropertyName("accountBaker")]
    public AccountBakerDetailsV0 Baker { get; init; }
}
