namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a keys version 0.
/// </summary>
public record KeysV0 : Keys
{
    /// <summary>
    /// Gets or initiates the level 2 keys.
    /// </summary>
    public AuthorizationsV0 Level2Keys { get; init; }
}
