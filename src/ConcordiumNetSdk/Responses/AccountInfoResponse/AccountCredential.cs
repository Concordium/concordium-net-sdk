namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the base class for an account credential.
/// </summary>
public abstract record AccountCredential
{
    /// <summary>
    /// Gets or initiates the account credential type.
    /// </summary>
    public string Type { get; init; }
}
