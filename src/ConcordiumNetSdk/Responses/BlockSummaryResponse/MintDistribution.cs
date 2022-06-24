namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for mint distribution.
/// </summary>
public abstract record MintDistribution
{
    /// <summary>
    /// Gets or initiates the baking reward.
    /// </summary>
    public decimal BakingReward { get; init; }

    /// <summary>
    /// Gets or initiates the finalization reward.
    /// </summary>
    public decimal FinalizationReward { get; init; }
}
