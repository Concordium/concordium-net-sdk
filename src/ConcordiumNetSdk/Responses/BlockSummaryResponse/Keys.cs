namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for keys.
/// </summary>
public abstract record Keys
{
    /// <summary>
    /// Gets or initiates the root keys.
    /// </summary>
    public KeysWithThreshold RootKeys { get; init; }

    /// <summary>
    /// Gets or initiates the level 1 keys.
    /// </summary>
    public KeysWithThreshold Level1Keys { get; init; }
}
