namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Account information exposed via the node's API. This is always the state of
/// an account in a specific block.
/// </summary>
public record AccountInfo
{
    /// <summary>
    /// Next nonce to be used for transactions signed from this account.
    /// </summary>
    public AccountSequenceNumber AccountNonce { get; init; }
    /// <summary>
    /// Current (unencrypted) balance of the account.
    /// </summary>
    public CcdAmount AccountAmount { get; init; }
    /// <summary>
    /// Internal index of the account. Accounts on the chain get sequential
    /// indices. These should generally not be used outside of the chain,
    /// the account address is meant to be used to refer to accounts,
    /// however the account index serves the role of the baker id, if the
    /// account is a baker. Hence it is exposed here as well.
    /// </summary>
    public ulong AccountIndex { get; init; }
    /// <summary>
    /// Canonical address of the account.
    /// </summary>
    public AccountAddress AccountAddress { get; init; }
    /// <summary>
    /// Not null if and only if the account is a baker or delegator. In that case
    /// it is the information about the baker or delegator.
    /// </summary>
    public IAccountStakingInfo? AccountStakingInfo { get; init; }

    private AccountInfo() {}

    internal static AccountInfo From(Grpc.V2.AccountInfo accountInfo) =>
        new()
        {
            AccountNonce = AccountSequenceNumber.From(accountInfo.SequenceNumber),
            AccountAmount = CcdAmount.From(accountInfo.Amount),
            AccountIndex = accountInfo.Index.Value,
            AccountAddress = AccountAddress.From(accountInfo.Address),
            AccountStakingInfo = Mapped.AccountStakingInfo.From(accountInfo.Stake)
        };
}
