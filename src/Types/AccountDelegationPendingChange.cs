using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Interface covering account delegation pending changes.
/// </summary>
public interface IAccountDelegationPendingChange{}

internal static class AccountDelegationPendingChangeFactory
{
    internal static IAccountDelegationPendingChange? From(StakePendingChange? stakeDelegatorPendingChange)
    {
        if (stakeDelegatorPendingChange == null)
        {
            return null;
        }

        return stakeDelegatorPendingChange.ChangeCase switch
        {
            StakePendingChange.ChangeOneofCase.Reduce =>
                new ReduceStakePending(
                    CcdAmount.From(stakeDelegatorPendingChange.Reduce.NewStake),
                    stakeDelegatorPendingChange.Reduce.EffectiveTime.ToDateTimeOffset()),
            StakePendingChange.ChangeOneofCase.Remove =>
                new RemoveStakePending(stakeDelegatorPendingChange.Remove.ToDateTimeOffset()),
            StakePendingChange.ChangeOneofCase.None =>
                throw new MissingEnumException<StakePendingChange.ChangeOneofCase>(stakeDelegatorPendingChange
                    .ChangeCase),
            _ => throw new MissingEnumException<StakePendingChange.ChangeOneofCase>(stakeDelegatorPendingChange
                .ChangeCase)
        };
    }
}

/// <summary>
/// The stake is being reduced. The new stake will take affect in the given time.
/// </summary>
/// <param name="EffectiveTime">Time when stake will be removed.</param>
public sealed record RemoveStakePending(DateTimeOffset EffectiveTime) : IAccountDelegationPendingChange;

/// <summary>
/// The baker will be removed at the end of the given time.
/// </summary>
/// <param name="NewStake">New stake after reduction.</param>
/// <param name="EffectiveTime">Time when new stake will take effect.</param>
public sealed record ReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : IAccountDelegationPendingChange;
