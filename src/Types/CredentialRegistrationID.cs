namespace Concordium.Sdk.Types;

/// <summary>
/// A registration ID of a credential. This ID is generated from the user's PRF
/// key and a sequential counter. CredentialRegistrationIDs generated from
/// the same PRF key, but different counter values cannot easily be linked
/// together.
/// </summary>
public sealed record CredentialRegistrationId(byte[] Id)
{
    /// <summary>
    /// Return hex string representation.
    /// </summary>
    /// <returns></returns>
    public string ToHex() => Convert.ToHexString(this.Id);

    internal static CredentialRegistrationId From(Grpc.V2.CredentialRegistrationId id) => new(id.Value.ToByteArray());
}
