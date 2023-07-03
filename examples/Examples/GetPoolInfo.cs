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

        var response = await this.Client.GetPoolInfoAsync(new BakerId(new AccountIndex(bakerId)), new Given(block));

        this.Output.WriteLine($"BlockHash: {response.BlockHash}");

        this.Output.WriteLine($"Baker {bakerId} has baker equity capital {response.Response.BakerEquityCapital.GetFormattedCcd()}");
    }
}
