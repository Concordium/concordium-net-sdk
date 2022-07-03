namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for stake pending change version 1.
/// </summary>
public abstract record StakePendingChangeV1 : StakePendingChange
{
    /// <summary>
    /// Gets or initiates the effective time.
    /// </summary>
    public DateTime EffectiveTime { get; init; }
}
