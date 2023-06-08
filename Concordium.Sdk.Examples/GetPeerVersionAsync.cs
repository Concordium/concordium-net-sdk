using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetPeerVersionAsync : Tests
{
    public GetPeerVersionAsync(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetPeerVersionAsync()
    {
        var peerVersion = await this.Client.GetPeerVersionAsync();

        this.Output.WriteLine($"Version of node was: {peerVersion}");
    }
}
