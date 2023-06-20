using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// A Sha256 with no specific meaning.
/// </summary>
public sealed record Sha256Hash : Hash
{
    internal Sha256Hash(ByteString byteString) : base(byteString.ToByteArray()){}
}
