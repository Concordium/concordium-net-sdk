namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: find out more info about class and all properties and their insides, can not add xml documentation
public abstract record AccountCredential
{
    // todo: implement type as enum or enum class
    public string Type { get; init; }
}
