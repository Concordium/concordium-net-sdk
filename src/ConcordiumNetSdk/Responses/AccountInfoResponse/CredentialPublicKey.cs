namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info for all properties and their insides, can not add xml documentation
public record CredentialPublicKey
{
    public string VerifyKey { get; init; }

    public string SchemeId { get; init; }
}
