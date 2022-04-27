namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info for all properties and their insides, can not add xml documentation
/// <summary>
/// Represents the initial credential values deployed on the account.
/// It is derived class from <see cref="SharedCredentialDeploymentValues"/> with date for general credential values.
/// </summary>
public record CredentialDeploymentValues : SharedCredentialDeploymentValues
{
    public string CredId { get; init; }
    
    public int RevocationThreshold { get; init; }

    public CredentialCommitments  Commitments  { get; init; }
    
    public Dictionary<int, ChainArData> ArData { get; init; }
}
