using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.NextAccountNonceResponse;

/// <summary>
/// Represents the next account sequence number.
/// </summary>
public record NextAccountNonce
{
    /// <summary>
    /// Gets or initiates the next account nonce.
    /// </summary>
    public Nonce Nonce { get; init; }

    /// <summary>
    /// Gets or initiates the indicator if all account transactions are finalized. 
    /// </summary>
    public bool AllFinal { get; init; }
}
