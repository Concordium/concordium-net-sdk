namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

//todo: think of making it as real class with own ctor and a base class for exchange rate
/// <summary>
/// Represents the information about a ratio.
/// </summary>
public record Ratio
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
