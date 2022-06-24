namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
    /// Represents the information about an exchange rate.
/// </summary>
public record ExchangeRate
{
    /// <summary>
    /// Gets or initiates the numerator.
    /// </summary>
    public ulong Numerator { get; init; }

    /// <summary>
    /// Gets or initiates the denominator.
    /// </summary>
    public ulong Denominator { get; init; }
}
