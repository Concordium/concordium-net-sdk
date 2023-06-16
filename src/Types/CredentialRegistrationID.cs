namespace Concordium.Sdk.Types;

/// <summary>
/// A registration ID of a credential. This ID is generated from the user's PRF
/// key and a sequential counter. [`CredentialRegistrationID`]'s generated from
/// the same PRF key, but different counter values cannot easily be linked
/// together.
/// </summary>
public record CredentialRegistrationId(byte[] Id)
{
    internal static CredentialRegistrationId From(Grpc.V2.CredentialRegistrationId id) => new(id.Value.ToByteArray());
}
