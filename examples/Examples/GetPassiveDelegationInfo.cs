using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetPassiveDelegationInfo : Tests
{
    public GetPassiveDelegationInfo(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetPassiveDelegationInfo()
    {
        var block = BlockHash.From(this.GetString("blockHash"));

        var response = await this.Client.GetPassiveDelegationInfoAsync(new Given(block));
        
        this.Output.WriteLine($"BlockHash: {response.BlockHash}");
        
        var poolStatus = response.Response;
        this.Output.WriteLine("The current commission rates are:");
        this.Output.WriteLine($"Baking Commission: {poolStatus.CommissionRates.BakingCommission}");
        this.Output.WriteLine($"Finalization Commission: {poolStatus.CommissionRates.FinalizationCommission}");
        this.Output.WriteLine($"Transaction Commission: {poolStatus.CommissionRates.TransactionCommission}");
    }
}
