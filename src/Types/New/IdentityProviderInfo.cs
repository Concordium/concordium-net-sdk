using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types.New;

public record IdentityProviderInfo(
    uint IpIdentity,
    ArOrIpDescription IpDescription,
    string IpVerifyKey,
    string IpCdiVerifyKey)
{
    internal static ValueTask<IdentityProviderInfo[]> From(IAsyncEnumerable<IpInfo> identityProviders)
    {
        return identityProviders
            .Select(i => IdentityProviderInfo.From(i))
            .ToArrayAsync();
    }

    private static IdentityProviderInfo From(IpInfo identityProviders)
    {
        throw new NotImplementedException();
    }
}
