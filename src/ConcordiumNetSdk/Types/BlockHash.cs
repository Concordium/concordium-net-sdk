namespace ConcordiumNetSdk.Types;

/// <summary>
/// A block hash.
/// </summary>
public class BlockHash : Hash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockHash"/> class.
    /// </summary>
    /// <param name="blockHashAsBase16String">A block hash represented as a length-64 hex encoded string.</param>
    private BlockHash(string blockHashAsBase16String)
        : base(blockHashAsBase16String) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockHash"/> class.
    /// </summary>
    /// <param name="blockHashAsBytes">A block hash represented as a length-32 byte array.</param>
    private BlockHash(byte[] blockHashAsBytes)
        : base(blockHashAsBytes) { }

    /// <summary>
    /// Creates an instance from a block hash represented by a length-64 hex encoded string.
    /// </summary>
    /// <param name="blockHashAsBase16String">A block hash represented as a length-64 hex encoded string.</param>
    public static BlockHash From(string blockHashAsBase16String)
    {
        return new BlockHash(blockHashAsBase16String);
    }

    /// <summary>
    /// Creates an instance from a block hash represented represented by a length-32 byte array.
    /// </summary>
    /// <param name="blockHashAsBytes">A block hash represented as a length-32 byte array.</param>
    public static BlockHash From(byte[] blockHashAsBytes)
    {
        return new BlockHash(blockHashAsBytes);
    }

    public Concordium.V2.BlockHash ToProto()
    {
        return new Concordium.V2.BlockHash()
        {
            Value = Google.Protobuf.ByteString.CopyFrom(this.GetBytes())
        };
    }
}
