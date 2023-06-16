namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// A single public key that can sign updates.
/// </summary>
public record UpdatePublicKey(byte[] Key)
{
    internal static UpdatePublicKey From(Grpc.V2.UpdatePublicKey key) => new(key.Value.ToByteArray());
}
