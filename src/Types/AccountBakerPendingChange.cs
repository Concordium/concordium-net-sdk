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
/// The baker will be removed at the end of the given epoch.
/// </summary>
/// <param name="Epoch">Epoch where baker will be removed.</param>
public record AccountBakerRemovePending(DateTimeOffset EffectiveTime) : AccountBakerPendingChange;
/// <summary>
/// The stake is being reduced. The new stake will take affect in the given
/// epoch.
/// </summary>
/// <param name="NewStake">New stake which will take effect.</param>
/// <param name="Epoch">Epoch where reduction will take place.</param>
public record AccountBakerReduceStakePending(CcdAmount NewStake, DateTimeOffset EffectiveTime) : AccountBakerPendingChange;
