using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for account baker details.
/// </summary>
public abstract record AccountBakerDetails
{
    /// <summary>
    /// Gets or initiates the restake earnings.
    /// </summary>
    public bool RestakeEarnings { get; init; }

    // todo: think of making BakerId class
    /// <summary>
    /// Gets or initiates the baker id.
    /// </summary>
    public ulong BakerId { get; init; }

    /// <summary>
    /// Gets or initiates the baker aggregation verify key.
    /// </summary>
    public string BakerAggregationVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the baker election verify key.
    /// </summary>
    public string BakerElectionVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the baker signature verify key.
    /// </summary>
    public string BakerSignatureVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the staked amount.
    /// </summary>
    public string StakedAmount { get; init; }

    /// <summary>
    /// Gets or initiates the pending change.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StakePendingChange? PendingChange { get; init; }
}
