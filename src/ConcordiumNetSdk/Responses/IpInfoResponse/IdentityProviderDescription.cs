namespace ConcordiumNetSdk.Responses.IpInfoResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about an identity provider description.
/// </summary>
public class IdentityProviderDescription
{
    /// <summary>
    /// Gets or initiates the name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets or initiates the url.
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// Gets or initiates the description.
    /// </summary>
    public string Description { get; init; }
}
