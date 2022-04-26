namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info about class and all properties and their insides, can not add xml documentation
public record NormalAccountCredential : AccountCredential
{
    public CredentialDeploymentValues Contents { get; init; }
}