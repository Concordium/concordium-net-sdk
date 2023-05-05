namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a transaction hash.
/// </summary>
public record TransactionHash : Hash
{
    private TransactionHash(string transactionHashAsBase16String)
        : base(transactionHashAsBase16String) { }

    private TransactionHash(byte[] transactionHashAsBytes)
        : base(transactionHashAsBytes) { }

    /// <summary>
    /// Creates an instance from a transaction hash represented by a length-64 encoded string.
    /// </summary>
    /// <param name="transactionHashAsBase16String">The transaction hash represented by a length-64 encoded string.</param>
    public static TransactionHash From(string transactionHashAsBase16String)
    {
        return new TransactionHash(transactionHashAsBase16String);
    }

    /// <summary>
    /// Creates an instance from a transaction hash represented by a length-32 byte array.
    /// </summary>
    /// <param name="transactionHashAsBytes">The transaction hash represented by a length-32 byte array</param>
    public static TransactionHash From(byte[] transactionHashAsBytes)
    {
        return new TransactionHash(transactionHashAsBytes);
    }

    /// <summary>
    /// Converts the transaction hash to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="ConcordiumNetSdk.Client.RawClient"/>.
    /// </summary>
    public Concordium.V2.TransactionHash ToProto()
    {
        return new Concordium.V2.TransactionHash()
        {
            Value = Google.Protobuf.ByteString.CopyFrom(this.GetBytes())
        };
    }
}
