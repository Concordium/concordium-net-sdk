namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: ask do we need to make RevealedAttributes key as a separate AttributeKey object can be value type (struct)
// todo: ask does AttributeKey have predefined keys and nothing else can be passed
// todo: or it is sth like auth token claims
// todo: find out more info for all properties and their insides, can not add xml documentation
public record Policy
{
    public string CreatedAt { get; init; }

    public string ValidTo { get; init; }

    public Dictionary<string, string> RevealedAttributes { get; init; }
}