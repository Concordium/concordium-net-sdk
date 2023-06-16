namespace Concordium.Sdk.Types;

/// <summary>
/// Information on a single anonymity revoker held by the IP.
/// Typically an IP will hold a more than one.
/// </summary>
/// <param name="ArIdentity">Unique identifier of the anonymity revoker</param>
/// <param name="ArDescription">Description of the anonymity revoker (e.g. name, contact number)</param>
/// <param name="ArPublicKey">Elgamal encryption key of the anonymity revoker</param>
public record ArInfo(ArIdentity ArIdentity, Description ArDescription, ArPublicKey ArPublicKey)
{
    internal static ArInfo From(Grpc.V2.ArInfo info) =>
        new(
            ArIdentity.From(info.Identity),
            Description.From(info.Description),
            ArPublicKey.From(info.PublicKey));
}
