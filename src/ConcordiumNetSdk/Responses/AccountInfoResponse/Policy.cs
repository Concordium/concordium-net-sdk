namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a policy.
/// </summary>
public record Policy
{
    /// <summary>
    /// Gets or initiates the created at.
    /// </summary>
    public string CreatedAt { get; init; }

    /// <summary>
    /// Gets or initiates the valid to.
    /// </summary>
    public string ValidTo { get; init; }

    /// <summary>
    /// Gets or initiates the revealed attributes.
    /// </summary>
    public Dictionary<string, string> RevealedAttributes { get; init; }
}
