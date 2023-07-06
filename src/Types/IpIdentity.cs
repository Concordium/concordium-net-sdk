namespace Concordium.Sdk.Types;

/// <summary>
/// A succinct identifier of an identity provider on the chain.
/// In credential deployments, and other interactions with the chain this is
/// used to identify which identity provider is meant.
/// </summary>
public readonly record struct IpIdentity(uint Id)
{
    internal static IpIdentity From(Grpc.V2.IpIdentity ipIdentity) => new(ipIdentity.Value);
}
