namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a normal account credential.
/// </summary>
public record NormalAccountCredential : AccountCredential
{
    /// <summary>
    /// Gets or initiates the contents.
    /// </summary>
    public CredentialDeploymentValues Contents { get; init; }
}
