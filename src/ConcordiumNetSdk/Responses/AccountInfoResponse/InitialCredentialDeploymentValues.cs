namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info for all properties and their insides, can not add xml documentation
/// <summary>
/// Represents the initial credential values deployed on the account.
/// It is derived class from <see cref="SharedCredentialDeploymentValues"/> with date for initial credential values.
/// </summary>
public record InitialCredentialDeploymentValues : SharedCredentialDeploymentValues
{
    public string RegId { get; init; }
}