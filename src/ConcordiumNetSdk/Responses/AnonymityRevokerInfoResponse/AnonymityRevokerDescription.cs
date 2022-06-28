namespace ConcordiumNetSdk.Responses.AnonymityRevokerInfoResponse;

/// <summary>
/// Represents the information about an anonymity revoker description.
/// </summary>
public record AnonymityRevokerDescription
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
