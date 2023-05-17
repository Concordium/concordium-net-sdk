using Concordium.Grpc.V2;
using Concordium.Sdk.Client;
using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a block hash.
///
/// A block hash is a 32-byte hash of a block and serves as
/// the canonical way of identifying it.
///
/// The address is usually provided as a hex encoded string.
/// </summary>
public record BlockHash : Hash
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
    /// Creates an instance from a byte string object given from generated code. Only internal use.
    /// </summary>
    /// <param name="byteString">A block hash represented as a immutable array.</param>
    internal static BlockHash From(ByteString byteString) => new(byteString.ToByteArray());

    /// <summary>
    /// Creates an instance from a block hash represented by a length-64 hex encoded string.
    /// </summary>
    /// <param name="blockHashAsBase16String">A block hash represented as a length-64 hex encoded string.</param>
    public static BlockHash From(string blockHashAsBase16String) => new(blockHashAsBase16String);

    /// <summary>
    /// Creates an instance from a block hash represented represented by a length-32 byte array.
    /// </summary>
    /// <param name="blockHashAsBytes">A block hash represented as a length-32 byte array.</param>
    public static BlockHash From(byte[] blockHashAsBytes) => new(blockHashAsBytes);

    /// <summary>
    /// Converts the block hash to its corresponding protocol buffer message instance.
    ///
    /// This can be used as input for class methods of <see cref="RawClient"/>.
    /// </summary>
    public Grpc.V2.BlockHash ToProto() => new()
    {
        Value = Google.Protobuf.ByteString.CopyFrom(this.GetBytes())
    };

    /// <summary>
    /// Converts the block hash to a corresponding <see cref="BlockHashInput"/>
    ///
    /// This can be used as the input for class methods of <see cref="RawClient"/>.
    /// </summary>
    public BlockHashInput ToBlockHashInput() => new() { Given = this.ToProto() };
}
