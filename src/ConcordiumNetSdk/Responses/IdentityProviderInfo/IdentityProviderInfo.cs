using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.IdentityProviderInfo;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about an identity provider in a specific block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetIdentityProvidersAsync"/>.
/// </summary>
public record IdentityProviderInfo
{
    /// <summary>
    /// Gets or initiates the identity provider identity.
    /// </summary>
    [JsonPropertyName("ipIdentity")]
    public int Identity { get; init; }

    /// <summary>
    /// Gets or initiates the identity provider description.
    /// </summary>
    [JsonPropertyName("ipDescription")]
    public IdentityProviderDescription Description { get; init; }

    /// <summary>
    /// Gets or initiates the identity provider verify key.
    /// </summary>
    [JsonPropertyName("ipVerifyKey")]
    public string VerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the identity provider cdi verify key.
    /// </summary>
    [JsonPropertyName("ipCdiVerifyKey")]
    public string CdiVerifyKey { get; init; }
}
