using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;
using Concordium.Sdk.Types.Baker;
using OpenStatus = Concordium.Sdk.Types.Baker.OpenStatus;

namespace Concordium.Sdk.Types.Events;

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
                new BakerAdded(BakerKeysEvent.From(bakerEvent.BakerAdded.KeysEvent), bakerEvent.BakerAdded.Stake.ToCcd(), bakerEvent.BakerAdded.RestakeEarnings),
            BakerEvent.EventOneofCase.BakerRemoved =>
                new BakerRemoved(BakerId.From(bakerEvent.BakerRemoved)),
            BakerEvent.EventOneofCase.BakerStakeIncreased =>
                new BakerStakeIncreased(BakerId.From(bakerEvent.BakerStakeIncreased.BakerId), bakerEvent.BakerStakeIncreased.NewStake.ToCcd()),
            BakerEvent.EventOneofCase.BakerStakeDecreased =>
                new BakerStakeDecreased(BakerId.From(bakerEvent.BakerStakeDecreased.BakerId), bakerEvent.BakerStakeDecreased.NewStake.ToCcd()),
            BakerEvent.EventOneofCase.BakerRestakeEarningsUpdated =>
                new BakerRestakeEarningsUpdated(BakerId.From(bakerEvent.BakerRestakeEarningsUpdated.BakerId), bakerEvent.BakerRestakeEarningsUpdated.RestakeEarnings),
            BakerEvent.EventOneofCase.BakerKeysUpdated =>
                new BakerKeysUpdated(BakerKeysEvent.From(bakerEvent.BakerKeysUpdated)),
            BakerEvent.EventOneofCase.BakerSetOpenStatus =>
                new BakerSetOpenStatus(BakerId.From(bakerEvent.BakerSetOpenStatus.BakerId),
                    bakerEvent.BakerSetOpenStatus.OpenStatus.Into()),
            BakerEvent.EventOneofCase.BakerSetMetadataUrl =>
                new BakerSetMetadataUrl(BakerId.From(bakerEvent.BakerSetMetadataUrl.BakerId), bakerEvent.BakerSetMetadataUrl.Url),
            BakerEvent.EventOneofCase.BakerSetTransactionFeeCommission =>
                new BakerSetTransactionFeeCommission(
                    BakerId.From(bakerEvent.BakerSetTransactionFeeCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetTransactionFeeCommission.TransactionFeeCommission)
                    ),
            BakerEvent.EventOneofCase.BakerSetBakingRewardCommission =>
                new BakerSetBakingRewardCommission(
                    BakerId.From(bakerEvent.BakerSetBakingRewardCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetBakingRewardCommission.BakingRewardCommission)
                ),
            BakerEvent.EventOneofCase.BakerSetFinalizationRewardCommission =>
                new BakerSetFinalizationRewardCommission(
                    BakerId.From(bakerEvent.BakerSetFinalizationRewardCommission.BakerId),
                    AmountFraction.From(bakerEvent.BakerSetFinalizationRewardCommission.FinalizationRewardCommission)
                ),
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
public record BakerAdded(BakerKeysEvent KeysEvent, CcdAmount Stake, bool RestakeEarnings) : IBakerEvent;

public record BakerRemoved(BakerId BakerId) : IBakerEvent;

public record BakerStakeIncreased(BakerId BakerId, CcdAmount NewStake) : IBakerEvent;

public record BakerStakeDecreased(BakerId BakerId, CcdAmount NewStake) : IBakerEvent;

/// <param name="RestakeEarnings">The new value of the flag.</param>
public record BakerRestakeEarningsUpdated(BakerId BakerId, bool RestakeEarnings) : IBakerEvent;

/// <summary>
/// The baker's keys were updated.
/// </summary>
public record BakerKeysUpdated(BakerKeysEvent Data) : IBakerEvent;

/// <summary>
/// Updated open status for a baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="OpenStatus">The open status.</param>
public record BakerSetOpenStatus(BakerId BakerId, OpenStatus OpenStatus) : IBakerEvent;

/// <summary>
/// Updated metadata url for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="MetadataUrl">The URL.</param>
public record BakerSetMetadataUrl(BakerId BakerId, string MetadataUrl) : IBakerEvent;

/// <summary>
/// Updated baking reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="TransactionFeeCommission">The baking reward commission</param>
public record BakerSetTransactionFeeCommission(BakerId BakerId, AmountFraction TransactionFeeCommission) : IBakerEvent;

/// <summary>
/// Updated baking reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="BakingRewardCommission">The baking reward commission</param>
public record BakerSetBakingRewardCommission(BakerId BakerId, AmountFraction BakingRewardCommission) : IBakerEvent;

/// <summary>
/// Updated finalization reward commission for baker pool
/// </summary>
/// <param name="BakerId">Baker's id</param>
/// <param name="FinalizationRewardCommission">The finalization reward commission</param>
public record BakerSetFinalizationRewardCommission(BakerId BakerId, AmountFraction FinalizationRewardCommission) : IBakerEvent;
