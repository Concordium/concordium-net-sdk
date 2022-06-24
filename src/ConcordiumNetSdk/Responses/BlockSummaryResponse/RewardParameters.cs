using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for reward parameters.
/// </summary>
public abstract record RewardParameters
{
    /// <summary>
    /// Gets or initiates the transaction fee distribution.
    /// </summary>
    public TransactionFeeDistribution TransactionFeeDistribution { get; init; }

    /// <summary>
    /// Gets or initiates the gas rewards.
    /// </summary>
    [JsonPropertyName("gASRewards")]
    public GasRewards GasRewards { get; init; }
}
