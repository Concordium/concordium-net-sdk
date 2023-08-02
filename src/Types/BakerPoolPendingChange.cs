using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Pending change in the baker's stake.
/// </summary>
public abstract record BakerPoolPendingChange
{
    internal static BakerPoolPendingChange? From(PoolPendingChange? pendingChange)
    {
        if (pendingChange is null)
        {
            return null;
        }

        return pendingChange.ChangeCase switch
        {
            PoolPendingChange.ChangeOneofCase.Reduce =>
                new BakerPoolReduceStakePending(CcdAmount.From(pendingChange.Reduce.ReducedEquityCapital), pendingChange.Reduce.EffectiveTime.ToDateTimeOffset()),
            PoolPendingChange.ChangeOneofCase.Remove =>
                new BakerPoolRemovePending(pendingChange.Remove.EffectiveTime.ToDateTimeOffset()),
            PoolPendingChange.ChangeOneofCase.None => null,
            _ => throw new MissingEnumException<PoolPendingChange.ChangeOneofCase>(pendingChange.ChangeCase)
        };
    }
}

/// <summary>
/// The baker will be removed at the given time.
/// </summary>
/// <param name="EffectiveTime">Time when the baker will be removed.</param>
public sealed record BakerPoolRemovePending(DateTimeOffset EffectiveTime) : BakerPoolPendingChange;
/// <summary>
/// The stake is being reduced. The new stake will take affect at the given time.
/// </summary>
/// <param name="NewStake">New stake which will take effect.</param>
/// <param name="EffectiveTime">Time when the baker will be removed.</param>
public sealed record BakerPoolReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : BakerPoolPendingChange;




