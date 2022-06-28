namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an initial account credential.
/// </summary>
public record InitialAccountCredential : AccountCredential
{
    /// <summary>
    /// Gets or initiates the contents.
    /// </summary>
    public InitialCredentialDeploymentValues Contents { get; init; }
}
