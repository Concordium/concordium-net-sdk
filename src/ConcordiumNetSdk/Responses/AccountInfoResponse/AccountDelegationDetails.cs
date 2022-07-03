namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account delegation details.
/// </summary>
public record AccountDelegationDetails
{
    /// <summary>
    /// Gets or initiates the restake earnings.
    /// </summary>
    public bool RestakeEarnings { get; init; }

    /// <summary>
    /// Gets or initiates the staked amount.
    /// </summary>
    public string StakedAmount { get; init; }

    /// <summary>
    /// Gets or initiates the delegation target.
    /// </summary>
    public DelegationTarget DelegationTarget { get; init; }

    /// <summary>
    /// Gets or initiates the pending change.
    /// </summary>
    public StakePendingChange? PendingChange { get; init; }
}
