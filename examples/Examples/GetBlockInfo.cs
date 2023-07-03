using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockInfo : Tests
{
    public GetBlockInfo(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetBlockInfoAsync()
    {
        var block = BlockHash.From(this.GetString("blockHash"));
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var response = await this.Client.GetBlockInfoAsync(new Given(block), cts.Token);

        this.Output.WriteLine($"Block: {response.BlockHash} has transaction energy cost: {response.Response.TransactionEnergyCost}");
    }
}
