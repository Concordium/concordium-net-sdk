namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a authorization.
/// </summary>
public record Authorization
{
    /// <summary>
    /// Gets or initiates the threshold.
    /// </summary>
    public int Threshold { get; init; }

    /// <summary>
    /// Gets or initiates the authorized keys.
    /// </summary>
    public List<int> AuthorizedKeys { get; init; }
}
