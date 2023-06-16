namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Public key of an anonymity revoker.
/// </summary>
public record ArPublicKey(byte[] Key)
{
    internal static ArPublicKey From(Grpc.V2.ArInfo.Types.ArPublicKey info) => new(info.Value.ToByteArray());
}
