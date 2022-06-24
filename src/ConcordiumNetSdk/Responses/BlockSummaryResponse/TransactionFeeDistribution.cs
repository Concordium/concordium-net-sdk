namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a transaction fee distribution.
/// </summary>
public record TransactionFeeDistribution
{
    /// <summary>
    /// Gets or initiates the baker.
    /// </summary>
    public decimal Baker { get; init; }

    /// <summary>
    /// Gets or initiates the gas account.
    /// </summary>
    public decimal GasAccount { get; init; }
}
