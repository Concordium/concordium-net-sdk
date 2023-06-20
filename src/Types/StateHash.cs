using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// Hash of the block state that is included in a block.
/// </summary>
public sealed record StateHash : Hash
{
    internal StateHash(ByteString bytes) : base(bytes.ToByteArray())
    {}
}
