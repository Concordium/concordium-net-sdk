namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base-16 encoded hash of a transaction.
/// </summary>
public class TransactionHash : Hash
{
    /// <summary>
    /// Creates an instance from a base-16 encoded string representing transaction hash (64 characters).
    /// </summary>
    /// <param name="transactionHashAsBase16String">the transaction hash as base16 encoded string.</param>  
    public TransactionHash(string transactionHashAsBase16String) : base(transactionHashAsBase16String)
    {
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing transaction hash.
    /// </summary>
    /// <param name="transactionHashAsBytes">the transaction hash as 32 bytes.</param>
    public TransactionHash(byte[] transactionHashAsBytes) : base(transactionHashAsBytes)
    {
    }
}
