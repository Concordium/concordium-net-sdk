namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a removal pending change version 0.
/// </summary>
public record RemovalPendingChangeV0 : StakePendingChangeV0
{
    /// <summary>
    /// Gets or initiates the stake pending change type.
    /// </summary>
    public StakePendingChangeType Change { get; init; } // StakePendingChangeType.RemoveStakeV0
}
