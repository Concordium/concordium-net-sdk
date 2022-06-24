namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a protocol update.
/// </summary>
public record ProtocolUpdate
{
    /// <summary>
    /// Gets or initiates the message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets or initiates the specification url.
    /// </summary>
    public string SpecificationUrl { get; init; }

    /// <summary>
    /// Gets or initiates the specification hash.
    /// </summary>
    public string SpecificationHash { get; init; }

    /// <summary>
    /// Gets or initiates the specification auxiliary data.
    /// </summary>
    public string SpecificationAuxiliaryData { get; init; }
}
