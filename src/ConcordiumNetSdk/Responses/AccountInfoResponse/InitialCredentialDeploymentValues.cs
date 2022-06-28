namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the initial credential values deployed on the account.
/// </summary>
public record InitialCredentialDeploymentValues : SharedCredentialDeploymentValues
{
    /// <summary>
    /// Gets or initiates the reg id.
    /// </summary>
    public string RegId { get; init; }
}
