using Google.Protobuf;

namespace Concordium.Sdk.Types;

public record HashBytes : Hash 
{
    internal HashBytes(ByteString byteString) : base(byteString.ToArray())
    { }
}