using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types.New;

namespace Concordium.Sdk.Types.Mapped;

public interface IAccountStakingInfo {}

internal static class AccountStakingInfo
{
    internal static IAccountStakingInfo? From(Grpc.V2.AccountStakingInfo accountStakingInfo) =>
        accountStakingInfo.StakingInfoCase switch
        {
            Grpc.V2.AccountStakingInfo.StakingInfoOneofCase.Baker => AccountBaker.From(accountStakingInfo
                .Baker),
            Grpc.V2.AccountStakingInfo.StakingInfoOneofCase.Delegator => AccountDelegation.From(
                accountStakingInfo.Delegator),
            Grpc.V2.AccountStakingInfo.StakingInfoOneofCase.None => null,
            _ => throw new MissingEnumException<Grpc.V2.AccountStakingInfo.StakingInfoOneofCase>(accountStakingInfo
                .StakingInfoCase)
        };
}

/// <summary>
/// The account is a baker.
/// </summary>
public class AccountBaker : IAccountStakingInfo
{
    public ulong BakerId { get; init; }
    public AccountBakerPendingChange? PendingChange { get; init; }
    public bool RestakeEarnings { get; init; }
    public CcdAmount StakedAmount { get; init; }
    public BakerPoolInfo? BakerPoolInfo  { get; init; }

    private AccountBaker() { }

    internal static AccountBaker From(Grpc.V2.AccountStakingInfo.Types.Baker stakeBaker)
    {
        return new AccountBaker
        {
            BakerId = stakeBaker.BakerInfo.BakerId.Value,
            PendingChange = AccountBakerPendingChange.From(stakeBaker.PendingChange),
            RestakeEarnings = stakeBaker.RestakeEarnings,
            StakedAmount = CcdAmount.From(stakeBaker.StakedAmount),
            BakerPoolInfo = BakerPoolInfo.From(stakeBaker.PoolInfo),
        };
    }
}

/// <summary>
/// The account is delegating stake to a baker.
/// </summary>
public class AccountDelegation : IAccountStakingInfo
{
    public bool RestakeEarnings { get; init; }
    public CcdAmount StakedAmount { get; init; }
    public DelegationTarget DelegationTarget { get; init; }
    public AccountDelegationPendingChange? PendingChange { get; init; }

    private AccountDelegation() {}

    internal static AccountDelegation From(Grpc.V2.AccountStakingInfo.Types.Delegator stakeDelegator) =>
        new()
        {
            RestakeEarnings = stakeDelegator.RestakeEarnings,
            StakedAmount = CcdAmount.From(stakeDelegator.StakedAmount),
            DelegationTarget = DelegationTarget.From(stakeDelegator.Target),
            PendingChange = AccountDelegationPendingChange.From(stakeDelegator.PendingChange)
        };
}
