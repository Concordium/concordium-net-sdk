namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a chain ar data.
/// </summary>
public record ChainArData
{
    /// <summary>
    /// Gets or initiates the enc id cred pub share. 
    /// </summary>
    public string EncIdCredPubShare { get; init; }
}
