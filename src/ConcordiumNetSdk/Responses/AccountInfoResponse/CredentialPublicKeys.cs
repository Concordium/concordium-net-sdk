namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info for all properties and their insides, can not add xml documentation
public record CredentialPublicKeys
{
    public Dictionary<int, CredentialPublicKey> Keys { get; init; }

    public int Threshold { get; init; }
}
