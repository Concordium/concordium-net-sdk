namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for credential values deployed on the account.
/// </summary>
public abstract record SharedCredentialDeploymentValues
{
    /// <summary>
    /// Gets or initiates the ip identity.
    /// </summary>
    public int IpIdentity { get; init; }

    /// <summary>
    /// Gets or initiates the credential public keys.
    /// </summary>
    public CredentialPublicKeys CredentialPublicKeys { get; init; }

    /// <summary>
    /// Gets or initiates the policy.
    /// </summary>
    public Policy Policy { get; init; }
}
