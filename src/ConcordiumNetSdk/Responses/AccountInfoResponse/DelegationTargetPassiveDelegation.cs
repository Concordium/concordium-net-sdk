namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a delegation target passive delegation.
/// </summary>
public record DelegationTargetPassiveDelegation : DelegationTarget
{
    /// <summary>
    /// Gets or initiates the delegation target type.
    /// </summary>
    public DelegationTargetType DelegateType { get; init; } // DelegationTargetType.PassiveDelegation
}
