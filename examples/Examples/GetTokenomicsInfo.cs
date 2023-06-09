using Concordium.Sdk.Types;
using Concordium.Sdk.Types.Mapped;
using Concordium.Sdk.Types.New;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetTokenomicsInfo : Tests
{
    public GetTokenomicsInfo(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetTokenomicsInfoAsync()
    {
        var block = BlockHash.From(this.GetString("blockHash"));

        var rewardOverview = await this.Client.GetTokenomicsInfoAsync(block);

        this.Output.WriteLine($"Protocol version: {rewardOverview.ProtocolVersion}");

        switch (rewardOverview)
        {
            case RewardOverviewV0 rewardOverviewV0:
                this.Output.WriteLine("This is reward version 0 return type.");
                this.Output.WriteLine($"Total CCD in existence: {rewardOverviewV0.TotalAmount}");
                break;
            case RewardOverviewV1 rewardOverviewV1:
                this.Output.WriteLine("This is reward version 1 return type.");
                this.Output.WriteLine($"Next payday mint rate: {rewardOverviewV1.NextPaydayMintRate.GetValue()}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rewardOverview));
        }
    }
}
