using Concordium.Sdk.Types.Mapped;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetIdentityProviders : Tests
{
    public GetIdentityProviders(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetIdentityProvidersAsync()
    {
        var identityProviders = await this.Client.GetIdentityProvidersAsync(new LastFinal());

        this.Output.WriteLine($"BlockHash: {identityProviders.BlockHash}");
        await foreach (var info in identityProviders.Response)
        {
            this.Output.WriteLine($"Id: {info.IpIdentity.Id}");
            this.Output.WriteLine($"Description info: {info.Description.Info}");
            this.Output.WriteLine($"Description name: {info.Description.Name}");
            this.Output.WriteLine($"Description url: {info.Description.Url}");
        }
    }
}
