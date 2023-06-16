namespace Concordium.Sdk.Types;

/// <summary>
/// Ed25519 public key of the identity provider.
/// </summary>
public record IpCdiVerifyKey(byte[] Key)
{
    internal static IpCdiVerifyKey From(Grpc.V2.IpInfo.Types.IpCdiVerifyKey ipVerifyKey) => new(ipVerifyKey.Value.ToByteArray());
}
