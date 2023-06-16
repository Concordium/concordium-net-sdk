using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetBlocksAtHeight : Tests
{
    public GetBlocksAtHeight(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetBlocksAtHeightAsync()
    {
        var info = await this.Client.GetConsensusInfoAsync();

        var absoluteHeight = new Absolute(info.BestBlockHeight);

        var blocks = await this.Client.GetBlocksAtHeightAsync(absoluteHeight, CancellationToken.None);

        this.Output.WriteLine($"Blocks live at height: {info.BestBlockHeight}");
        foreach (var block in blocks)
        {
            this.Output.WriteLine(block.ToString());
        }
    }
}
