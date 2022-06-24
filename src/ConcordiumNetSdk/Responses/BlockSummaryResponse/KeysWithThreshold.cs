namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a keys with threshold.
/// </summary>
public record KeysWithThreshold
{
    /// <summary>
    /// Gets or initiates the keys.
    /// </summary>
    public List<VerifyKey> Keys { get; init; }

    /// <summary>
    /// Gets or initiates the threshold.
    /// </summary>
    public int Threshold { get; init; }
}
