namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a credential public keys.
/// </summary>
public record CredentialPublicKeys
{
    /// <summary>
    /// Gets or initiates the keys.
    /// </summary>
    public Dictionary<int, CredentialPublicKey> Keys { get; init; }

    /// <summary>
    /// Gets or initiates the threshold.
    /// </summary>
    public int Threshold { get; init; }
}
