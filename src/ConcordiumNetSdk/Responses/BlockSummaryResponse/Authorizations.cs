using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for authorizations.
/// </summary>
public abstract record Authorizations
{
    /// <summary>
    /// Gets or initiates the emergency.
    /// </summary>
    public Authorization Emergency { get; init; }

    /// <summary>
    /// Gets or initiates the micro gtu per euro.
    /// </summary>
    [JsonPropertyName("microGTUPerEuro")]
    public Authorization MicroGtuPerEuro { get; init; }

    /// <summary>
    /// Gets or initiates the euro per energy.
    /// </summary>
    public Authorization EuroPerEnergy { get; init; }

    /// <summary>
    /// Gets or initiates the transaction fee distribution.
    /// </summary>
    public Authorization TransactionFeeDistribution { get; init; }

    /// <summary>
    /// Gets or initiates the foundation account.
    /// </summary>
    public Authorization FoundationAccount { get; init; }

    /// <summary>
    /// Gets or initiates the mint distribution.
    /// </summary>
    public Authorization MintDistribution { get; init; }

    /// <summary>
    /// Gets or initiates the protocol.
    /// </summary>
    public Authorization Protocol { get; init; }

    /// <summary>
    /// Gets or initiates the param gas rewards.
    /// </summary>
    [JsonPropertyName("paramGASRewards")]
    public Authorization ParamGasRewards { get; init; }

    /// <summary>
    /// Gets or initiates the pool parameters.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Authorization? PoolParameters { get; init; }

    /// <summary>
    /// Gets or initiates the baker stake threshold.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Authorization? BakerStakeThreshold { get; init; }

    /// <summary>
    /// Gets or initiates the election difficulty.
    /// </summary>
    public Authorization ElectionDifficulty { get; init; }

    /// <summary>
    /// Gets or initiates the add anonymity revoker.
    /// </summary>
    public Authorization AddAnonymityRevoker { get; init; }

    /// <summary>
    /// Gets or initiates the add identity provider.
    /// </summary>
    public Authorization AddIdentityProvider { get; init; }

    /// <summary>
    /// Gets or initiates the keys.
    /// </summary>
    public List<VerifyKey> Keys { get; init; }
}
