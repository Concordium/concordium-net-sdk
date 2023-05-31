namespace Concordium.Sdk.Types.New;

public class AccountDelegation
{
    public bool RestakeEarnings { get; init; }
    public CcdAmount StakedAmount { get; init; }
    public DelegationTarget DelegationTarget { get; init; }
    public AccountDelegationPendingChange? PendingChange { get; init; }
}