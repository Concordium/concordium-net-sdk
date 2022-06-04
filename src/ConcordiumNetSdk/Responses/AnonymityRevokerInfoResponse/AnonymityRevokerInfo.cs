using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AnonymityRevokerInfoResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about an anonymity revoker in a specific block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetAnonymityRevokersAsync"/>.
/// </summary>
public record AnonymityRevokerInfo
{
    /// <summary>
    /// Gets or initiates the anonymity revoker identity.
    /// </summary>
    [JsonPropertyName("arIdentity")]
    public int Identity { get; init; }

    /// <summary>
    /// Gets or initiates the anonymity revoker description.
    /// </summary>
    [JsonPropertyName("arDescription")]
    public AnonymityRevokerDescription Description { get; init; }

    /// <summary>
    /// Gets or initiates the anonymity revoker public key.
    /// </summary>
    [JsonPropertyName("arPublicKey")]
    public string PublicKey { get; init; }
}
