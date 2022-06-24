namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a reward parameters version 0.
/// </summary>
public record RewardParametersV0 : RewardParameters
{
    /// <summary>
    /// Gets or initiates the mint distribution.
    /// </summary>
    public MintDistributionV0 MintDistribution { get; init; }
}
