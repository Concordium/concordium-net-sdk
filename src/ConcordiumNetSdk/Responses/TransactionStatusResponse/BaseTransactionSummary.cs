namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a base transaction summary.
/// </summary>
public record BaseTransactionSummary : TransactionSummary
{
    /// <summary>
    /// Gets or initiates the sender.
    /// </summary>
    public string? Sender { get; init; }

    /// <summary>
    /// Gets or initiates the hash.
    /// </summary>
    public string Hash { get; init; }

    //todo: use long or ulong for it, investigate serialization and deserialization, make custom converter
    /// <summary>
    /// Gets or initiates the cost.
    /// </summary>
    public string Cost { get; init; }

    /// <summary>
    /// Gets or initiates the energy cost.
    /// </summary>
    public ulong EnergyCost { get; init; }

    /// <summary>
    /// Gets or initiates the index.
    /// </summary>
    public ulong Index { get; init; }
}
