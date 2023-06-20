namespace Concordium.Sdk.Types;

/// <summary>
/// A single public key that can sign updates.
/// </summary>
public sealed record UpdatePublicKey(byte[] Key)
{
    internal static UpdatePublicKey From(Grpc.V2.UpdatePublicKey key) => new(key.Value.ToByteArray());
}
