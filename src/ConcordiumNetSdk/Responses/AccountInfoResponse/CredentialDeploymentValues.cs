namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the initial credential values deployed on the account.
/// </summary>
public record CredentialDeploymentValues : SharedCredentialDeploymentValues
{
    /// <summary>
    /// Gets or initiates the cred id. 
    /// </summary>
    public string CredId { get; init; }
    
    /// <summary>
    /// Gets or initiates the revocation threshold. 
    /// </summary>
    public int RevocationThreshold { get; init; }

    /// <summary>
    /// Gets or initiates the commitments. 
    /// </summary>
    public CredentialDeploymentCommitments  Commitments  { get; init; }
    
    /// <summary>
    /// Gets or initiates the ar data. 
    /// </summary>
    public Dictionary<int, ChainArData> ArData { get; init; }
}
