using Concordium.Sdk.Types.Mapped;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockTransactionEvents : Tests
{
    public GetBlockTransactionEvents(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetBlockTransactionEvents()
    {
        var idx = 0UL;
        var awaitResult = true;
        while (awaitResult)
        {
            var blockHeight = new Absolute(idx);
            var finalizationSummary = this.Client.GetBlockTransactionEvents(blockHeight);
            if (finalizationSummary == null)
            {
                idx++;
                continue;
            }
            awaitResult = false;
            //
            // this.Output.WriteLine($"At height {idx} block {finalizationSummary.BlockPointer} had finalization summary");
            // this.Output.WriteLine($"Finalization round index: {finalizationSummary.Index}, finalization delay: {finalizationSummary.Delay}");
            // this.Output.WriteLine($"With finalizers");
            // foreach (var party in finalizationSummary.Finalizers)
            // {
            //     this.Output.WriteLine($"Baker: {party.BakerId}, weight in committee: {party.Weight}");
            // }
        }
    }
}
