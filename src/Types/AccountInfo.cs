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
public sealed record AccountInfo(
    AccountSequenceNumber AccountNonce,
    CcdAmount AccountAmount,
    AccountIndex AccountIndex,
    AccountAddress AccountAddress,
    IAccountStakingInfo? AccountStakingInfo)
{
    internal static AccountInfo From(Grpc.V2.AccountInfo accountInfo) =>
        new(
            AccountNonce: AccountSequenceNumber.From(accountInfo.SequenceNumber),
            AccountAmount: CcdAmount.From(accountInfo.Amount),
            AccountIndex: new AccountIndex(accountInfo.Index.Value),
            AccountAddress: AccountAddress.From(accountInfo.Address),
            AccountStakingInfo: Types.AccountStakingInfo.From(accountInfo.Stake)
        );
}
