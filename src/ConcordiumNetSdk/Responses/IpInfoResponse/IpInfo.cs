namespace ConcordiumNetSdk.Responses.IpInfoResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about an identity provider in a specific block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetIdentityProvidersAsync"/>.
/// </summary>
public record IpInfo
{
    /// <summary>
    /// Gets or initiates the ip identity.
    /// </summary>
    public int IpIdentity { get; init; }

    /// <summary>
    /// Gets or initiates the ip description.
    /// </summary>
    public IpDescription IpDescription { get; init; }

    /// <summary>
    /// Gets or initiates the ip verify key.
    /// </summary>
    public string IpVerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the ip cdi verify key.
    /// </summary>
    public string IpCdiVerifyKey { get; init; }
}
