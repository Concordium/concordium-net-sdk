using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetPoolInfo : Tests
{
    public GetPoolInfo(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetPoolInfoAsync()
    {
        var block = BlockHash.From(this.GetString("blockHash"));
        const ulong bakerId = 1;

        var poolInfo = await this.Client.GetPoolInfoAsync(bakerId, block);

        this.Output.WriteLine($"Baker {bakerId} has baker equity capital {poolInfo.BakerEquityCapital.GetFormattedCcd()}");
    }
}
