using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a credential deployment commitments.
/// </summary>
public record CredentialDeploymentCommitments
{
    /// <summary>
    /// Gets or initiates the prf. 
    /// </summary>
    [JsonPropertyName("cmmPrf")]
    public string Prf { get; init; }

    /// <summary>
    /// Gets or initiates the cred counter. 
    /// </summary>
    [JsonPropertyName("cmmCredCounter")]
    public string CredCounter { get; init; }

    /// <summary>
    /// Gets or initiates the id cred sec sharing coeff. 
    /// </summary>
    [JsonPropertyName("cmmIdCredSecSharingCoeff")]
    public List<string> IdCredSecSharingCoeff { get; init; }

    /// <summary>
    /// Gets or initiates the attributes. 
    /// </summary>
    [JsonPropertyName("cmmAttributes")]
    public Dictionary<string, string> Attributes { get; init; }

    /// <summary>
    /// Gets or initiates the max accounts. 
    /// </summary>
    [JsonPropertyName("cmmMaxAccounts")]
    public string MaxAccounts { get; init; }
}
