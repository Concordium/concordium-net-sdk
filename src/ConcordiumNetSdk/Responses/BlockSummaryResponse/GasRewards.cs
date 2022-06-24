namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a gas rewards.
/// </summary>
public record GasRewards
{
    /// <summary>
    /// Gets or initiates the baker.
    /// </summary>
    public decimal Baker { get; init; }

    /// <summary>
    /// Gets or initiates the finalization proof.
    /// </summary>
    public decimal FinalizationProof { get; init; }

    /// <summary>
    /// Gets or initiates the account creation.
    /// </summary>
    public decimal AccountCreation { get; init; }

    /// <summary>
    /// Gets or initiates the chain update.
    /// </summary>
    public decimal ChainUpdate { get; init; }
}
