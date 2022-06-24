using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for update queues.
/// </summary>
public abstract record UpdateQueues
{
    /// <summary>
    /// Gets or initiates the micro gtu per euro.
    /// </summary>
    [JsonPropertyName("microGTUPerEuro")]
    public UpdateQueue MicroGtuPerEuro { get; init; }

    /// <summary>
    /// Gets or initiates the euro per energy.
    /// </summary>
    public UpdateQueue EuroPerEnergy { get; init; }

    /// <summary>
    /// Gets or initiates the transaction fee distribution.
    /// </summary>
    public UpdateQueue TransactionFeeDistribution { get; init; }

    /// <summary>
    /// Gets or initiates the foundation account.
    /// </summary>
    public UpdateQueue FoundationAccount { get; init; }

    /// <summary>
    /// Gets or initiates the election difficulty.
    /// </summary>
    public UpdateQueue ElectionDifficulty { get; init; }

    /// <summary>
    /// Gets or initiates the mint distribution.
    /// </summary>
    public UpdateQueue MintDistribution { get; init; }

    /// <summary>
    /// Gets or initiates the protocol.
    /// </summary>
    public UpdateQueue Protocol { get; init; }

    /// <summary>
    /// Gets or initiates the gas rewards.
    /// </summary>
    public UpdateQueue GasRewards { get; init; }

    /// <summary>
    /// Gets or initiates the add anonymity revoker.
    /// </summary>
    public UpdateQueue AddAnonymityRevoker { get; init; }

    /// <summary>
    /// Gets or initiates the add identity provider.
    /// </summary>
    public UpdateQueue AddIdentityProvider { get; init; }

    /// <summary>
    /// Gets or initiates the root keys.
    /// </summary>
    public UpdateQueue RootKeys { get; init; }

    /// <summary>
    /// Gets or initiates the level 1 keys.
    /// </summary>
    public UpdateQueue Level1Keys { get; init; }

    /// <summary>
    /// Gets or initiates the level 2 keys.
    /// </summary>
    public UpdateQueue Level2Keys { get; init; }
}
