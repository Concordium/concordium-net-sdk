using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

public abstract record AccountDelegationPendingChange
{
    public static AccountDelegationPendingChange? From(StakePendingChange? stakeDelegatorPendingChange)
    {
        if (stakeDelegatorPendingChange == null)
        {
            return null;
        }

        return stakeDelegatorPendingChange.ChangeCase switch
        {
            StakePendingChange.ChangeOneofCase.Reduce =>
                new AccountDelegationReduceStakePending(
                    CcdAmount.From(stakeDelegatorPendingChange.Reduce.NewStake),
                    stakeDelegatorPendingChange.Reduce.EffectiveTime.ToDateTimeOffset()),
            StakePendingChange.ChangeOneofCase.Remove =>
                new AccountDelegationRemovePending(stakeDelegatorPendingChange.Remove.ToDateTimeOffset()),
            StakePendingChange.ChangeOneofCase.None =>
                throw new MissingEnumException<StakePendingChange.ChangeOneofCase>(stakeDelegatorPendingChange
                    .ChangeCase),
            _ => throw new MissingEnumException<StakePendingChange.ChangeOneofCase>(stakeDelegatorPendingChange
                .ChangeCase)
        };
    }
}

public sealed record AccountDelegationRemovePending(DateTimeOffset EffectiveTime) : AccountDelegationPendingChange;

public sealed record AccountDelegationReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : AccountDelegationPendingChange;
