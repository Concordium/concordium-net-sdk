namespace Concordium.Sdk.Types.New;

public abstract record AccountDelegationPendingChange;

public record AccountDelegationRemovePending(DateTimeOffset EffectiveTime) : AccountDelegationPendingChange;

public record AccountDelegationReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : AccountDelegationPendingChange;