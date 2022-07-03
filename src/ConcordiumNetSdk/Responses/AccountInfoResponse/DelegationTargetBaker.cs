namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a delegation target baker.
/// </summary>
public record DelegationTargetBaker : DelegationTarget
{
    /// <summary>
    /// Gets or initiates the delegation target type.
    /// </summary>
    public DelegationTargetType DelegateType { get; init; } // DelegationTargetType.Baker

    // todo: think of making BakerId class
    /// <summary>
    /// Gets or initiates the baker id.
    /// </summary>
    public ulong BakerId { get; init; }
}
