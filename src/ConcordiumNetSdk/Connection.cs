namespace ConcordiumNetSdk;

public record Connection
{
    public string Address { get; init; }

    public string AuthenticationToken { get; init; }
}
