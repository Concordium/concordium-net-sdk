namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a keys version 1.
/// </summary>
public record KeysV1 : Keys
{
    /// <summary>
    /// Gets or initiates the level 2 keys.
    /// </summary>
    public AuthorizationsV1 Level2Keys { get; init; }
}
