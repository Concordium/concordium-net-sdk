using Google.Protobuf;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Hash of the block state that is included in a block.
/// </summary>
public record StateHash : Hash
{
    internal StateHash(ByteString bytes) : base(bytes.ToByteArray())
    {}
}
