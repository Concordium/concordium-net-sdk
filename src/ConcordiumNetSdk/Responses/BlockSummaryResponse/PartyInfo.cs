namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a party.
/// </summary>
public record PartyInfo
{
    /// <summary>
    /// Gets or initiates the baker id.
    /// </summary>
    public ulong BakerId { get; init; }

    /// <summary>
    /// Gets or initiates the weight.
    /// </summary>
    public ulong Weight { get; init; }

    /// <summary>
    /// Gets or initiates the is signed flag.
    /// </summary>
    public bool Signed { get; init; }
}
