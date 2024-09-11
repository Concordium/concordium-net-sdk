namespace Concordium.Sdk.Types;

/// <summary>
/// Account information exposed via the node's API. This is always the state of
/// an account in a specific block.
/// </summary>
/// <param name="AccountNonce">Next nonce to be used for transactions signed from this account.</param>
/// <param name="AccountAmount">Current (unencrypted) balance of the account.</param>
/// <param name="AccountIndex">
/// Internal index of the account. Accounts on the chain get sequential
/// indices. These should generally not be used outside of the chain,
/// the account address is meant to be used to refer to accounts,
/// however the account index serves the role of the baker id, if the
/// account is a baker. Hence it is exposed here as well.
/// </param>
/// <param name="AccountAddress">Canonical address of the account.</param>
/// <param name="AccountStakingInfo">
/// Not null if and only if the account is a baker or delegator. In that case
/// it is the information about the baker or delegator.
/// </param>
/// <param name="Schedule">
/// Release schedule for any locked up amount. This could be an empty
/// release schedule.
/// </param>
/// <param name="Cooldowns">
/// The stake on the account that is in cooldown.
/// There can be multiple amounts in cooldown that expire at different times.
/// This was introduced in protocol version 7, and so is not present in
/// earlier protocol versions.
/// </param>
/// <param name="AvailableBalance">
/// The available (unencrypted) balance of the account (i.e. that can be transferred
/// or used to pay for transactions). This is the balance minus the locked amount.
/// The locked amount is the maximum of the amount in the release schedule and
/// the total amount that is actively staked or in cooldown (inactive stake).
/// </param>
public sealed record AccountInfo(
    AccountSequenceNumber AccountNonce,
    CcdAmount AccountAmount,
    AccountIndex AccountIndex,
    AccountAddress AccountAddress,
    IAccountStakingInfo? AccountStakingInfo,
    ReleaseSchedule Schedule,
    IList<Cooldown> Cooldowns,
    CcdAmount AvailableBalance
)
{
    internal static AccountInfo From(Grpc.V2.AccountInfo accountInfo)
    {
        var accountAmount = CcdAmount.From(accountInfo.Amount);
        var schedule = ReleaseSchedule.From(accountInfo.Schedule);
        var stakingInfo = Types.AccountStakingInfo.From(accountInfo.Stake);

        // `accountInfo.availableBalance` was introduce in the Concordium Node API v7,
        // so to remain backwards compatible with node versions prior to this, we
        // compute the available balance if this is not present.
        CcdAmount availableBalance;
        if (accountInfo.AvailableBalance == null)
        {
            var staked = stakingInfo?.GetStakedAmount() ?? CcdAmount.Zero;
            availableBalance = accountAmount - CcdAmount.Max(staked, schedule.Total);
        }
        else
        {
            availableBalance = CcdAmount.From(accountInfo.AvailableBalance);
        }

        return new(
            AccountNonce: AccountSequenceNumber.From(accountInfo.SequenceNumber),
            AccountAmount: accountAmount,
            AccountIndex: new AccountIndex(accountInfo.Index.Value),
            AccountAddress: AccountAddress.From(accountInfo.Address),
            AccountStakingInfo: stakingInfo,
            Schedule: schedule,
            Cooldowns: accountInfo.Cooldowns.Select(Cooldown.From).ToList(),
            AvailableBalance: availableBalance
        );
    }
}
