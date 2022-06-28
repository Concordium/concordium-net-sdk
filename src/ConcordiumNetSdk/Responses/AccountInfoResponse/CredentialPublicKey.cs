namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a credential public key.
/// </summary>
public record CredentialPublicKey
{
    /// <summary>
    /// Gets or initiates the verify key. 
    /// </summary>
    public string VerifyKey { get; init; }

    /// <summary>
    /// Gets or initiates the scheme id. 
    /// </summary>
    public string SchemeId { get; init; }
}
