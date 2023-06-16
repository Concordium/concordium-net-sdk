namespace Concordium.Sdk.Types;

/// <summary>
/// Pointcheval-Sanders public key of the identity provider.
/// </summary>
public record IpVerifyKey(byte[] Key)
{
    internal static IpVerifyKey From(Grpc.V2.IpInfo.Types.IpVerifyKey ipVerifyKey) => new(ipVerifyKey.Value.ToByteArray());
}
