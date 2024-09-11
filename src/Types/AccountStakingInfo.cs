using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Information related to staking for a specific account.
/// </summary>
public interface IAccountStakingInfo { }

internal static class AccountStakingInfo
{
    internal static IAccountStakingInfo? From(Grpc.V2.AccountStakingInfo? accountStakingInfo)
    {
        if (accountStakingInfo is null)
        {
            return null;
        }

        return accountStakingInfo.StakingInfoCase switch
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
}

/// <summary>
/// The account is a baker.
/// </summary>
public sealed record AccountBaker(
    bool RestakeEarnings,
    CcdAmount StakedAmount,
    BakerInfo BakerInfo,
    AccountBakerPendingChange? PendingChange,
    BakerPoolInfo? BakerPoolInfo
    ) : IAccountStakingInfo
{
    internal static AccountBaker From(Grpc.V2.AccountStakingInfo.Types.Baker stakeBaker) =>
        new(
            BakerInfo: BakerInfo.From(stakeBaker.BakerInfo),
            PendingChange: AccountBakerPendingChange.From(stakeBaker.PendingChange),
            RestakeEarnings: stakeBaker.RestakeEarnings,
            StakedAmount: CcdAmount.From(stakeBaker.StakedAmount),
            BakerPoolInfo: BakerPoolInfo.From(stakeBaker.PoolInfo)
        );
}

/// <summary>
/// The account is delegating stake to a baker.
/// </summary>
public sealed record AccountDelegation(
    bool RestakeEarnings,
    CcdAmount StakedAmount,
    DelegationTarget DelegationTarget,
    IAccountDelegationPendingChange? PendingChange) : IAccountStakingInfo
{
    internal static AccountDelegation From(Grpc.V2.AccountStakingInfo.Types.Delegator stakeDelegator) =>
        new
        (
            RestakeEarnings: stakeDelegator.RestakeEarnings,
            StakedAmount: CcdAmount.From(stakeDelegator.StakedAmount),
            DelegationTarget: DelegationTarget.From(stakeDelegator.Target),
            PendingChange: AccountDelegationPendingChangeFactory.From(stakeDelegator.PendingChange)
        );
}
