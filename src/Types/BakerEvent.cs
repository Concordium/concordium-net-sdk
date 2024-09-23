using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Events that may result from the <see cref="TransactionType.ConfigureBaker"/>
/// transaction.
/// </summary>
public interface IBakerEvent
{
}

internal static class BakerEventFactory
{
    internal static IBakerEvent From(BakerEvent bakerEvent) =>
        bakerEvent.EventCase switch
        {
            BakerEvent.EventOneofCase.BakerAdded =>
                new BakerAddedEvent(BakerKeysEvent.From(bakerEvent.BakerAdded.KeysEvent), bakerEvent.BakerAdded.Stake.ToCcd(), bakerEvent.BakerAdded.RestakeEarnings),
            BakerEvent.EventOneofCase.BakerRemoved =>
                new BakerRemovedEvent(BakerId.From(bakerEvent.BakerRemoved)),
            BakerEvent.EventOneofCase.BakerStakeIncreased =>
                new BakerStakeIncreasedEvent(BakerId.From(bakerEvent.BakerStakeIncreased.BakerId), bakerEvent.BakerStakeIncreased.NewStake.ToCcd()),
            BakerEvent.EventOneofCase.BakerStakeDecreased =>
                new BakerStakeDecreasedEvent(BakerId.From(bakerEvent.BakerStakeDecreased.BakerId), bakerEvent.BakerStakeDecreased.NewStake.ToCcd()),
            BakerEvent.EventOneofCase.BakerRestakeEarningsUpdated =>
                new BakerRestakeEarningsUpdatedEvent(BakerId.From(bakerEvent.BakerRestakeEarningsUpdated.BakerId), bakerEvent.BakerRestakeEarningsUpdated.RestakeEarnings),
            BakerEvent.EventOneofCase.BakerKeysUpdated =>
                new BakerKeysUpdatedEvent(BakerKeysEvent.From(bakerEvent.BakerKeysUpdated)),
            BakerEvent.EventOneofCase.BakerSetOpenStatus =>
                new BakerSetOpenStatusEvent(BakerId.From(bakerEvent.BakerSetOpenStatus.BakerId),
                    bakerEvent.BakerSetOpenStatus.OpenStatus.Into()),
            BakerEvent.EventOneofCase.BakerSetMetadataUrl =>
                new BakerSetMetadataUrlEvent(BakerId.From(bakerEvent.BakerSetMetadataUrl.BakerId), bakerEvent.BakerSetMetadataUrl.Url),
            BakerEvent.EventOneofCase.BakerSetTransactionFeeCommission =>
                new BakerSetTransactionFeeCommissionEvent(
                    BakerId.From(bakerEvent.BakerSetTransactionFeeCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetTransactionFeeCommission.TransactionFeeCommission)
                    ),
            BakerEvent.EventOneofCase.BakerSetBakingRewardCommission =>
                new BakerSetBakingRewardCommissionEvent(
                    BakerId.From(bakerEvent.BakerSetBakingRewardCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetBakingRewardCommission.BakingRewardCommission)
                ),
            BakerEvent.EventOneofCase.BakerSetFinalizationRewardCommission =>
                new BakerSetFinalizationRewardCommissionEvent(
                    BakerId.From(bakerEvent.BakerSetFinalizationRewardCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetFinalizationRewardCommission.FinalizationRewardCommission)
                ),
            BakerEvent.EventOneofCase.DelegationRemoved => new BakerEventDelegationRemoved(DelegatorId.From(bakerEvent.DelegationRemoved.DelegatorId)),
            BakerEvent.EventOneofCase.None =>
                throw new MissingEnumException<BakerEvent.EventOneofCase>(bakerEvent.EventCase),
            _ => throw new MissingEnumException<BakerEvent.EventOneofCase>(bakerEvent.EventCase)
        };
}

/// <summary>
/// Event created when baker was added.
/// </summary>
/// <param name="KeysEvent">The keys with which the baker registered.</param>
/// <param name="Stake">
/// The amount the account staked to become a baker. This amount is
/// locked.
/// </param>
/// <param name="RestakeEarnings">
/// Whether the baker will automatically add earnings to their stake or
/// not.
/// </param>
public sealed record BakerAddedEvent(BakerKeysEvent KeysEvent, CcdAmount Stake, bool RestakeEarnings) : IBakerEvent;

/// <summary>
/// Baker was removed.
/// </summary>
/// <param name="BakerId">Baker Id of removed baker</param>
public sealed record BakerRemovedEvent(BakerId BakerId) : IBakerEvent;

/// <summary>
/// Stake increased on baker.
/// </summary>
/// <param name="BakerId">Baker Id</param>
/// <param name="NewStake">Stake after increase.</param>
public sealed record BakerStakeIncreasedEvent(BakerId BakerId, CcdAmount NewStake) : IBakerEvent;

/// <summary>
/// Stake decreased on baker.
/// </summary>
/// <param name="BakerId">Baker Id</param>
/// <param name="NewStake">Stake after decrease.</param>
public sealed record BakerStakeDecreasedEvent(BakerId BakerId, CcdAmount NewStake) : IBakerEvent;

/// <summary>
/// Changed if earnings should be restaked.
/// </summary>
/// <param name="BakerId">Baker Id</param>
/// <param name="RestakeEarnings">The new value of the flag.</param>
public sealed record BakerRestakeEarningsUpdatedEvent(BakerId BakerId, bool RestakeEarnings) : IBakerEvent;

/// <summary>
/// The baker's keys were updated.
/// </summary>
public sealed record BakerKeysUpdatedEvent(BakerKeysEvent Data) : IBakerEvent;

/// <summary>
/// Updated open status for a baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="OpenStatus">The open status.</param>
public sealed record BakerSetOpenStatusEvent(BakerId BakerId, BakerPoolOpenStatus OpenStatus) : IBakerEvent;

/// <summary>
/// Updated metadata url for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="MetadataUrl">The URL.</param>
public sealed record BakerSetMetadataUrlEvent(BakerId BakerId, string MetadataUrl) : IBakerEvent;

/// <summary>
/// Updated baking reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="TransactionFeeCommission">The baking reward commission</param>
public sealed record BakerSetTransactionFeeCommissionEvent(BakerId BakerId, AmountFraction TransactionFeeCommission) : IBakerEvent;

/// <summary>
/// Updated baking reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="BakingRewardCommission">The baking reward commission</param>
public sealed record BakerSetBakingRewardCommissionEvent(BakerId BakerId, AmountFraction BakingRewardCommission) : IBakerEvent;

/// <summary>
/// Updated finalization reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="FinalizationRewardCommission">The finalization reward commission</param>
public sealed record BakerSetFinalizationRewardCommissionEvent(BakerId BakerId, AmountFraction FinalizationRewardCommission) : IBakerEvent;

/// <summary>
/// An existing delegator was removed.
/// </summary>
/// <param name="DelegatorId">Delegator's id</param>
public sealed record BakerEventDelegationRemoved(DelegatorId DelegatorId) : IBakerEvent;
