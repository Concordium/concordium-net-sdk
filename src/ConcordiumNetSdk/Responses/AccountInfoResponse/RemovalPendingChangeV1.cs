namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a removal pending change version 1.
/// </summary>
public record RemovalPendingChangeV1 : StakePendingChangeV1
{
    /// <summary>
    /// Gets or initiates the stake pending change type.
    /// </summary>
    public StakePendingChangeType Change { get; init; } // StakePendingChangeType.RemoveStakeV1;
}
