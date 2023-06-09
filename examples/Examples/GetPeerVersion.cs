using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetPeerVersion : Tests
{
    public GetPeerVersion(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetPeerVersionAsync()
    {
        var peerVersion = await this.Client.GetPeerVersionAsync();

        this.Output.WriteLine($"Version of node was: {peerVersion}");
    }
}
