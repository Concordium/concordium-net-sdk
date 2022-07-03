namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for reduce stake pending change.
/// </summary>
public interface IReduceStakePendingChange
{
    /// <summary>
    /// Gets or initiates the new stake.
    /// </summary>
    long NewStake { get; init; }
}   
