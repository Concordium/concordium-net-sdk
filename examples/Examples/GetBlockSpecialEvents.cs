using Concordium.Sdk.Types.Mapped;
using Xunit.Abstractions;
using BlockHash = Concordium.Sdk.Types.BlockHash;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockSpecialEvents : Tests
{
    public GetBlockSpecialEvents(ITestOutputHelper output) : base(output) {}

    [Fact]
    public async Task RunGetBlockSpecialEvents()
    {
        var block = BlockHash.From(this.GetString("blockHashWithEvents"));
        await foreach(var specialEvent in this.Client.GetBlockSpecialEvents(new Given(block)))
        {
            foreach (var accountBalanceUpdate in specialEvent.GetAccountBalanceUpdates())
            {
                this.Output.WriteLine($"Type of special event: {specialEvent.GetType().Name}");
                this.Output.WriteLine($"Account {accountBalanceUpdate.AccountAddress} with adjustment {accountBalanceUpdate.AmountAdjustment} and balance update type {accountBalanceUpdate.BalanceUpdateType.ToString()}");
            }
        }
    }
}
