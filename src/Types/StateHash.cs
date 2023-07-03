using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// Hash of the block state that is included in a block.
/// </summary>
public sealed record StateHash : Hash
{
    /// <summary>
    /// Create state hash from hex string.
    /// </summary>
    public StateHash(string hexString) : base(hexString)
    { }

    internal StateHash(ByteString bytes) : base(bytes.ToByteArray())
    { }
}
