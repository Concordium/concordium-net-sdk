namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for stake pending change version 0.
/// </summary>
public abstract record StakePendingChangeV0 : StakePendingChange
{
    /// <summary>
    /// Gets or initiates the epoch.
    /// </summary>
    public long Epoch { get; init; }
}
