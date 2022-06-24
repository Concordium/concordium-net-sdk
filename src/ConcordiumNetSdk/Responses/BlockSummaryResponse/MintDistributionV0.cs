namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a mint distribution version 0.
/// </summary>
public record MintDistributionV0 : MintDistribution
{
    /// <summary>
    /// Gets or initiates the mint per slot.
    /// </summary>
    public decimal MintPerSlot { get; init; }
}
