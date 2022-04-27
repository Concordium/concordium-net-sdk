using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: think do we need to make CmmAttributes key as a separate AttributeKey object can be value type (struct) look at js repo
// todo: ask does AttributeKey have predefined keys and nothing else can be passed 
// todo: or it is sth like auth token claims
// todo: find out more info for all properties and their insides, can not add xml documentation
public record CredentialCommitments
{
    [JsonPropertyName("cmmPrf")]
    public string Prf { get; init; }

    [JsonPropertyName("cmmCredCounter")]
    public string CredCounter { get; init; }

    [JsonPropertyName("cmmIdCredSecSharingCoeff")]
    public List<string> IdCredSecSharingCoeff { get; init; }

    [JsonPropertyName("cmmAttributes")]
    public Dictionary<string, string> Attributes { get; init; }

    [JsonPropertyName("cmmMaxAccounts")]
    public string MaxAccounts { get; init; }
}
