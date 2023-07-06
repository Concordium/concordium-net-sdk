using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Pending change in the baker's stake.
/// </summary>
public abstract record AccountBakerPendingChange
{
    internal static AccountBakerPendingChange? From(StakePendingChange? stakeBakerPendingChange)
    {
        if (stakeBakerPendingChange is null)
        {
            return null;
        }

        return stakeBakerPendingChange.ChangeCase switch
        {
            StakePendingChange.ChangeOneofCase.Reduce =>
                new AccountBakerReduceStakePending(CcdAmount.From(stakeBakerPendingChange.Reduce.NewStake), stakeBakerPendingChange.Remove.ToDateTimeOffset()),
            StakePendingChange.ChangeOneofCase.Remove =>
                new AccountBakerRemovePending(stakeBakerPendingChange.Remove.ToDateTimeOffset())
            ,
            StakePendingChange.ChangeOneofCase.None => null,
            _ => throw new MissingEnumException<StakePendingChange.ChangeOneofCase>(stakeBakerPendingChange.ChangeCase)
        };
    }
}

/// <summary>
/// The baker will be removed at the given time.
/// </summary>
/// <param name="EffectiveTime">Time when the baker will be removed.</param>
public sealed record AccountBakerRemovePending(DateTimeOffset EffectiveTime) : AccountBakerPendingChange;
/// <summary>
/// The stake is being reduced. The new stake will take affect at the given time.
/// </summary>
/// <param name="NewStake">New stake which will take effect.</param>
/// <param name="EffectiveTime">Time when the baker will be removed.</param>
public sealed record AccountBakerReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : AccountBakerPendingChange;
