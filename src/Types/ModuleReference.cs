using Google.Protobuf;
namespace Concordium.Sdk.Types;

/// <summary>
/// A reference to a smart contract module deployed on the chain.
/// </summary>
public record ModuleReference : Hash
{
    internal ModuleReference(ByteString byteString) : base(byteString.ToArray())
    { }

    public ModuleReference(string hash) : base(hash)
    {}
}
