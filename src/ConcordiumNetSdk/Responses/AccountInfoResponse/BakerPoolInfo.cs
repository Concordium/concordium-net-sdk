namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a baker pool.
/// </summary>
public record BakerPoolInfo
{
    /// <summary>
    /// Gets or initiates the open status.
    /// </summary>
    public OpenStatusText OpenStatus { get; init; }

    /// <summary>
    /// Gets or initiates the metadata url.
    /// </summary>
    public string MetadataUrl { get; init; }

    /// <summary>
    /// Gets or initiates the commission rates.
    /// </summary>
    public CommissionRates CommissionRates { get; init; }
}
