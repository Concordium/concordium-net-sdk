using Concordium.Sdk.Types;
using Concordium.Sdk.Types.Mapped;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetAccountInfoAsync : Tests
{
    public GetAccountInfoAsync(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetAccountInfoAsync()
    {
        var accountAddress = AccountAddress.From(this.GetString("accountAddress"));
        var block = BlockHash.From(this.GetString("blockHash"));

        var accountInfo = await this.Client.GetAccountInfoAsync(accountAddress, block);

        if (accountInfo.AccountStakingInfo is null)
        {
            this.Output.WriteLine($"Address: {accountInfo.AccountAddress} doesn't stake");
            return;
        }

        switch (accountInfo.AccountStakingInfo)
        {
            case AccountBaker accountBaker:
                this.Output.WriteLine($"Account is baker with staked CCD amount: {accountBaker.StakedAmount.GetFormattedCcd()}.");
                break;
            case AccountDelegation accountDelegation:
                this.Output.WriteLine($"Account is delegating CCD amount: {accountDelegation.StakedAmount.GetFormattedCcd()}.");

                if (accountDelegation.PendingChange is null)
                {
                    break;
                }

                switch (accountDelegation.PendingChange)
                {
                    case AccountDelegationReduceStakePending reduce:
                        this.Output.WriteLine($"At {reduce.EffectiveTime} new stake wil be {reduce.NewStake.GetFormattedCcd()}.");
                        break;
                    case AccountDelegationRemovePending remove:
                        this.Output.WriteLine($"At {remove.EffectiveTime} stake will be removed.");
                        break;
                    default:
                        throw CreateArgumentOutOfRangeException(accountDelegation.PendingChange);
                }

                break;
            default:
                throw CreateArgumentOutOfRangeException(accountInfo.AccountStakingInfo);
        }
    }
}
