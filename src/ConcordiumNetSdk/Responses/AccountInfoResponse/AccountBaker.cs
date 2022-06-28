namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account baker.
/// </summary>
public record AccountBaker
{
    /// <summary>
    /// Gets or initiates the baker id.
    /// </summary>
    public int BakerId { get; init; }

    /// <summary>
    /// Gets or initiates the staked amount.
    /// </summary>
    public string StakedAmount { get; init; }

    /// <summary>
    /// Gets or initiates the restake earnings.
    /// </summary>
    public bool RestakeEarnings { get; init; }

    /// <summary>
    /// Gets or initiates the baker election verify key.
    /// </summary>
    public string BakerElectionVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the baker signature verify key.
    /// </summary>
    public string BakerSignatureVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the baker aggregation verify key.
    /// </summary>
    public string BakerAggregationVerifyKey { get; init; }
}
