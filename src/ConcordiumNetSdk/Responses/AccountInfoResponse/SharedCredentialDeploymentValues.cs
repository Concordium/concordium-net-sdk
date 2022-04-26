namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info for all properties and their insides, can not add xml documentation
/// <summary>
/// Represents the credential values deployed on the account.
/// It is base class with base date for credential values.
/// </summary>
public abstract record SharedCredentialDeploymentValues
{
    public int IpIdentity { get; init; }

    public CredentialPublicKeys CredentialPublicKeys { get; init; }

    public Policy Policy { get; init; }
}