namespace ConcordiumNetSdk.Responses.IdentityProviderInfoResponse;

/// <summary>
/// Represents the information about an identity provider description.
/// </summary>
public record IdentityProviderDescription
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
