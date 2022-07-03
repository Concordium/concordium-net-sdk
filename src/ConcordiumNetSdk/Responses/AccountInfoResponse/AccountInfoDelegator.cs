namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account delegator.
/// </summary>
public record AccountInfoDelegator : AccountInfo
{
    /// <summary>
    /// Gets or initiates the account delegation details.
    /// </summary>
    public AccountDelegationDetails AccountDelegation { get; init; }
}
