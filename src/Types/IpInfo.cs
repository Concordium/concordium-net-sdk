namespace Concordium.Sdk.Types;

/// <summary>
/// Public information about an identity provider.
/// </summary>
/// <param name="IpIdentity">Unique identifier of the identity provider.</param>
/// <param name="Description">Free form description, e.g., how to contact them off-chain</param>
/// <param name="IpVerifyKey">PS public key of the IP</param>
/// <param name="IpCdiVerifyKey">Ed public key of the IP</param>
public record IpInfo(IpIdentity IpIdentity, Description Description, IpVerifyKey IpVerifyKey, IpCdiVerifyKey IpCdiVerifyKey)
{
    internal static IpInfo From(Grpc.V2.IpInfo info) =>
        new(
            IpIdentity.From(info.Identity),
            Description.From(info.Description),
            IpVerifyKey.From(info.VerifyKey),
            IpCdiVerifyKey.From(info.CdiVerifyKey));
}
