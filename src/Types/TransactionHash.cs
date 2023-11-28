namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a transaction hash.
/// </summary>
public sealed record TransactionHash : Hash
{
    private TransactionHash(string transactionHashAsBase16String)
        : base(transactionHashAsBase16String) { }

    private TransactionHash(byte[] transactionHashAsBytes)
        : base(transactionHashAsBytes) { }

    /// <summary>
    /// Creates an instance from a transaction hash represented by a length-64 encoded string.
    /// </summary>
    /// <param name="transactionHashAsBase16String">The transaction hash represented by a length-64 encoded string.</param>
    public static TransactionHash From(string transactionHashAsBase16String) =>
        new(transactionHashAsBase16String);

    /// <summary>
    /// Creates an instance from a transaction hash represented by a length-32 byte array.
    /// </summary>
    /// <param name="transactionHashAsBytes">The transaction hash represented by a length-32 byte array</param>
    public static TransactionHash From(byte[] transactionHashAsBytes) =>
        new(transactionHashAsBytes);

    internal static TransactionHash From(Grpc.V2.TransactionHash proto) =>
        new(proto.Value.ToArray());

    /// <summary>
    /// Converts the transaction hash to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="Client.RawClient"/>.
    /// </summary>
    public Grpc.V2.TransactionHash ToProto() =>
        new()
        {
            Value = Google.Protobuf.ByteString.CopyFrom(this.ToBytes())
        };
}
