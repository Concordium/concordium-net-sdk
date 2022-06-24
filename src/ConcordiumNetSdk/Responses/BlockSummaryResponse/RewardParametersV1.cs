namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a reward parameters version 1.
/// </summary>
public record RewardParametersV1 : RewardParameters
{
    /// <summary>
    /// Gets or initiates the mint distribution.
    /// </summary>
    public MintDistributionV1 MintDistribution { get; init; }
}
