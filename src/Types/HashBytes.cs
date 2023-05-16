using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// A general wrapper of a 32-size hash. This is used to add type safety to
/// a hash that is used in different context.
/// </summary>
public record HashBytes : Hash
{
    internal HashBytes(ByteString byteString) : base(byteString.ToArray())
    { }
}
