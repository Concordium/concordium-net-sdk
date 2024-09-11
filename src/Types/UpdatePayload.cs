using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;
using UpdatePayload = Concordium.Grpc.V2.UpdatePayload;

namespace Concordium.Sdk.Types;

/// <summary>
/// The type of an update payload.
/// </summary>
public interface IUpdatePayload
{
}

/// <summary>
/// The type of an update.
/// </summary>
public enum UpdateType
{
    /// <summary> Update of protocol version. </summary>
    ProtocolUpdate,
    /// <summary> Update of the election difficulty. </summary>
    ElectionDifficultyUpdate,
    /// <summary> Update of conversion rate of Euro per energy. </summary>
    EuroPerEnergyUpdate,
    /// <summary> Update of conversion rate of CCD per Euro. </summary>
    MicroCcdPerEuroUpdate,
    /// <summary> Update of account marked as foundation account. </summary>
    FoundationAccountUpdate,
    /// <summary> Update of distribution of minted CCD. </summary>
    MintDistributionUpdate,
    /// <summary> Update of distribution of transaction fee. </summary>
    TransactionFeeDistributionUpdate,
    /// <summary> Update of distribution of GAS rewards. </summary>
    GasRewardsUpdate,
    /// <summary> Update of minimum threshold for becoming a validator. </summary>
    BakerStakeThresholdUpdate,
    /// <summary> Introduce new Identity Disclosure Authority. </summary>
    AddAnonymityRevokerUpdate,
    /// <summary> Introduce new Identity Provider. </summary>
    AddIdentityProviderUpdate,
    /// <summary> Update of root keys. </summary>
    RootUpdate,
    /// <summary> Update of level1 keys. </summary>
    Level1Update,
    /// <summary> Update of level2 keys. </summary>
    Level2Update,
    /// <summary> Update of pool parameters. </summary>
    PoolParametersCpv1Update,
    /// <summary> Update of cooldown parameters. </summary>
    CooldownParametersCpv1Update,
    /// <summary> Update of time parameters. </summary>
    TimeParametersCpv1Update,
    /// <summary> Update of distribution of minted CCD. </summary>
    MintDistributionCpv1Update,
    /// <summary> Update of distribution of GAS rewards. </summary>
    GasRewardsCpv2Update,
    /// <summary> Update of timeout parameters. </summary>
    TimeoutParametersUpdate,
    /// <summary> Update of min-block-time parameters. </summary>
    MinBlockTimeUpdate,
    /// <summary> Update of block energy limit parameters. </summary>
    BlockEnergyLimitUpdate,
    /// <summary> Update of finalization committee parameters. </summary>
    FinalizationCommitteeParametersUpdate,
}

/// <summary>
/// Helper for constructing update payload structures.
/// </summary>
public static class UpdatePayloadFactory
{
    /// <summary>
    /// Get update payload enum type for update payload type.
    /// </summary>
    /// <exception cref="MissingTypeException{IUpdatePayload}">When type can't be matched to enum.</exception>
    public static UpdateType From(IUpdatePayload payload) =>
        payload switch
        {
            AddAnonymityRevokerUpdate => UpdateType.AddAnonymityRevokerUpdate,
            AddIdentityProviderUpdate => UpdateType.AddIdentityProviderUpdate,
            BakerStakeThresholdUpdate => UpdateType.BakerStakeThresholdUpdate,
            BlockEnergyLimitUpdate => UpdateType.BlockEnergyLimitUpdate,
            CooldownParametersCpv1Update => UpdateType.CooldownParametersCpv1Update,
            ElectionDifficultyUpdate => UpdateType.ElectionDifficultyUpdate,
            EuroPerEnergyUpdate => UpdateType.EuroPerEnergyUpdate,
            FinalizationCommitteeParametersUpdate =>
                UpdateType.FinalizationCommitteeParametersUpdate,
            FoundationAccountUpdate => UpdateType.FoundationAccountUpdate,
            GasRewardsCpv2Update => UpdateType.GasRewardsCpv2Update,
            GasRewardsUpdate => UpdateType.GasRewardsUpdate,
            RootUpdate => UpdateType.RootUpdate,
            Level1 => UpdateType.Level1Update,
            MicroCcdPerEuroUpdate => UpdateType.MicroCcdPerEuroUpdate,
            MinBlockTimeUpdate => UpdateType.MinBlockTimeUpdate,
            MintDistributionCpv0Update => UpdateType.MintDistributionUpdate,
            MintDistributionCpv1Update => UpdateType.MintDistributionCpv1Update,
            PoolParametersCpv1Update => UpdateType.PoolParametersCpv1Update,
            ProtocolUpdate => UpdateType.ProtocolUpdate,
            TimeoutParametersUpdate => UpdateType.TimeoutParametersUpdate,
            TimeParametersCpv1Update => UpdateType.TimeParametersCpv1Update,
            TransactionFeeDistributionUpdate => UpdateType.TransactionFeeDistributionUpdate,
            _ => throw new MissingTypeException<IUpdatePayload>(payload)
        };

    internal static IUpdatePayload From(UpdatePayload payload) =>
        payload.PayloadCase switch
        {
            UpdatePayload.PayloadOneofCase.ProtocolUpdate =>
                ProtocolUpdate.From(payload.ProtocolUpdate),
            UpdatePayload.PayloadOneofCase.ElectionDifficultyUpdate =>
                ElectionDifficultyUpdate.From(payload.ElectionDifficultyUpdate),
            UpdatePayload.PayloadOneofCase.EuroPerEnergyUpdate =>
                EuroPerEnergyUpdate.From(payload.EuroPerEnergyUpdate),
            UpdatePayload.PayloadOneofCase.MicroCcdPerEuroUpdate =>
                MicroCcdPerEuroUpdate.From(payload.MicroCcdPerEuroUpdate),
            UpdatePayload.PayloadOneofCase.FoundationAccountUpdate =>
                FoundationAccountUpdate.From(payload.FoundationAccountUpdate),
            UpdatePayload.PayloadOneofCase.MintDistributionUpdate =>
                MintDistributionCpv0Update.From(payload.MintDistributionUpdate),
            UpdatePayload.PayloadOneofCase.TransactionFeeDistributionUpdate =>
                TransactionFeeDistributionUpdate.From(payload.TransactionFeeDistributionUpdate),
            UpdatePayload.PayloadOneofCase.GasRewardsUpdate =>
                GasRewardsUpdate.From(payload.GasRewardsUpdate),
            UpdatePayload.PayloadOneofCase.BakerStakeThresholdUpdate =>
                BakerStakeThresholdUpdate.From(payload.BakerStakeThresholdUpdate),
            UpdatePayload.PayloadOneofCase.RootUpdate =>
                RootUpdate.From(payload.RootUpdate),
            UpdatePayload.PayloadOneofCase.Level1Update =>
                Level1.From(payload.Level1Update),
            UpdatePayload.PayloadOneofCase.AddAnonymityRevokerUpdate =>
                AddAnonymityRevokerUpdate.From(payload.AddAnonymityRevokerUpdate),
            UpdatePayload.PayloadOneofCase.AddIdentityProviderUpdate =>
                AddIdentityProviderUpdate.From(payload.AddIdentityProviderUpdate),
            UpdatePayload.PayloadOneofCase.CooldownParametersCpv1Update =>
                CooldownParametersCpv1Update.From(payload.CooldownParametersCpv1Update),
            UpdatePayload.PayloadOneofCase.PoolParametersCpv1Update =>
                PoolParametersCpv1Update.From(payload.PoolParametersCpv1Update),
            UpdatePayload.PayloadOneofCase.TimeParametersCpv1Update =>
                TimeParametersCpv1Update.From(payload.TimeParametersCpv1Update),
            UpdatePayload.PayloadOneofCase.MintDistributionCpv1Update =>
                MintDistributionCpv1Update.From(payload.MintDistributionCpv1Update),
            UpdatePayload.PayloadOneofCase.GasRewardsCpv2Update =>
                GasRewardsCpv2Update.From(payload.GasRewardsCpv2Update),
            UpdatePayload.PayloadOneofCase.TimeoutParametersUpdate =>
                TimeoutParametersUpdate.From(payload.TimeoutParametersUpdate),
            UpdatePayload.PayloadOneofCase.MinBlockTimeUpdate =>
                MinBlockTimeUpdate.From(payload.MinBlockTimeUpdate),
            UpdatePayload.PayloadOneofCase.BlockEnergyLimitUpdate =>
                BlockEnergyLimitUpdate.From(payload.BlockEnergyLimitUpdate),
            UpdatePayload.PayloadOneofCase.FinalizationCommitteeParametersUpdate =>
                FinalizationCommitteeParametersUpdate.From(payload.FinalizationCommitteeParametersUpdate),
            UpdatePayload.PayloadOneofCase.None =>
            throw new MissingEnumException<UpdatePayload.PayloadOneofCase>(payload.PayloadCase),
            _ => throw new MissingEnumException<UpdatePayload.PayloadOneofCase>(payload.PayloadCase)
        };
}

/// <summary>
/// A generic protocol update. This is essentially an announcement of the
/// update. The details of the update will be communicated in some off-chain
/// way, and bakers will need to update their node software to support the
/// update.
/// </summary>
public sealed record ProtocolUpdate(string Message, string SpecificationUrl, Sha256Hash SpecificationHash, byte[] SpecificationAuxiliaryData) : IUpdatePayload
{
    internal static ProtocolUpdate From(Grpc.V2.ProtocolUpdate update) =>
        new(
            update.Message,
            update.SpecificationUrl,
            new Sha256Hash(update.SpecificationHash.Value),
            update.SpecificationAuxiliaryData.ToByteArray()
        );
}

/// <summary>
/// Representation of the election difficulty as parts per `100_000`. The
/// election difficulty is never more than `1`.
/// </summary>
public sealed record ElectionDifficultyUpdate(AmountFraction PartsPerHundredThousands) : IUpdatePayload
{
    internal static ElectionDifficultyUpdate From(Grpc.V2.ElectionDifficulty fraction) =>
        new(AmountFraction.From(fraction.Value));
}

/// <summary>
/// Exchange rate for euro pr. energy.
/// </summary>
/// <param name="ExchangeRate"></param>
public sealed record EuroPerEnergyUpdate(ExchangeRate ExchangeRate) : IUpdatePayload
{
    internal static EuroPerEnergyUpdate From(Grpc.V2.ExchangeRate rate) => new(ExchangeRate.From(rate));
}

/// <summary>
/// Exchange rate for micro ccd pr. euro.
/// </summary>
/// <param name="ExchangeRate"></param>
public sealed record MicroCcdPerEuroUpdate(ExchangeRate ExchangeRate) : IUpdatePayload
{
    internal static MicroCcdPerEuroUpdate From(Grpc.V2.ExchangeRate rate) => new(ExchangeRate.From(rate));
}

/// <summary>
/// Update of the foundation account.
/// </summary>
public sealed record FoundationAccountUpdate(AccountAddress AccountAddress) : IUpdatePayload
{
    internal static FoundationAccountUpdate From(Grpc.V2.AccountAddress address) => new(AccountAddress.From(address));
}

/// <summary>
/// Mint distribution that applies to protocol versions 1-3 with chain parameters version 0.
/// </summary>
/// <param name="MintDistribution">Updated min distribution parameters.</param>
public sealed record MintDistributionCpv0Update(MintDistributionCpv0 MintDistribution) : IUpdatePayload
{
    internal static MintDistributionCpv0Update From(Grpc.V2.MintDistributionCpv0 chainParameterVersion0) =>
        new(MintDistributionCpv0.From(chainParameterVersion0));
}

/// <summary>
/// Update the transaction fee distribution to the specified value.
/// </summary>
/// <param name="TransactionFeeDistribution">The transaction fee distribution updates.</param>
public sealed record TransactionFeeDistributionUpdate(TransactionFeeDistribution TransactionFeeDistribution) : IUpdatePayload
{
    internal static TransactionFeeDistributionUpdate From(Grpc.V2.TransactionFeeDistribution distribution) =>
        new(TransactionFeeDistribution.From(distribution));
}

/// <summary>
/// The reward fractions related to the gas account and  inclusion of special
/// transactions.
/// </summary>
/// <param name="GasRewards">Gas rewards updates.</param>
public sealed record GasRewardsUpdate(GasRewards GasRewards) : IUpdatePayload
{
    internal static GasRewardsUpdate From(Grpc.V2.GasRewards rewards) =>
        new(GasRewards.From(rewards));
}

/// <summary>
/// Parameters related to becoming a baker that apply to protocol versions 1-3.
/// </summary>
/// <param name="MinimumThresholdForBaking">Minimum amount of CCD that an account must stake to become a baker.</param>
public sealed record BakerStakeThresholdUpdate(CcdAmount MinimumThresholdForBaking) : IUpdatePayload
{
    internal static BakerStakeThresholdUpdate From(Grpc.V2.BakerStakeThreshold threshold) =>
        new(threshold.BakerStakeThreshold_.ToCcd());
}

/// <summary>
/// An update with root keys of some other set of governance keys, or the root
/// keys themselves.
/// </summary>
public sealed record RootUpdate(IRootUpdate Update) : IUpdatePayload
{
    internal static RootUpdate From(Grpc.V2.RootUpdate root) => new(RootUpdateFactory.From(root));
}

/// <summary>
/// Update to anonymity revoker.
/// </summary>
public sealed record AddAnonymityRevokerUpdate(ArInfo ArInfo) : IUpdatePayload
{
    internal static AddAnonymityRevokerUpdate From(Grpc.V2.ArInfo info) =>
        new(ArInfo.From(info));
}

/// <summary>
/// Update to identity provider.
/// </summary>
public sealed record AddIdentityProviderUpdate(IpInfo IpInfo) : IUpdatePayload
{
    internal static AddIdentityProviderUpdate From(Grpc.V2.IpInfo info) => new(IpInfo.From(info));
}

/// <summary>
/// Update to cooldown parameters with chain parameter version 1.
/// </summary>
public sealed record CooldownParametersCpv1Update(CooldownParameters CooldownParameters) : IUpdatePayload
{
    internal static CooldownParametersCpv1Update From(Grpc.V2.CooldownParametersCpv1 cooldown) =>
        new(CooldownParameters.From(cooldown));
}

/// <summary>
/// Update to pool- and delegation parameters.
/// </summary>
public sealed record PoolParametersCpv1Update(PoolParameters PoolParameters) : IUpdatePayload
{
    internal static PoolParametersCpv1Update From(Grpc.V2.PoolParametersCpv1 pool) =>
        new(PoolParameters.From(pool));
}

/// <summary>
/// Update to mint rate pr day and reward period length.
/// </summary>
/// <param name="TimeParameters"></param>
public sealed record TimeParametersCpv1Update(TimeParameters TimeParameters) : IUpdatePayload
{
    internal static TimeParametersCpv1Update From(Grpc.V2.TimeParametersCpv1 timeParametersCpv1) => new(TimeParameters.From(timeParametersCpv1));
}

/// <summary>
/// Mint distribution parameters that apply to protocol version 4 and up. with chain parameters version 1.
/// </summary>
/// <param name="BakingReward">Fraction of newly minted CCD allocated to baker rewards.</param>
/// <param name="FinalizationReward">Fraction of newly minted CCD allocated to finalization rewards.</param>
public sealed record MintDistributionCpv1Update(AmountFraction BakingReward, AmountFraction FinalizationReward) : IUpdatePayload
{
    internal static MintDistributionCpv1Update From(Grpc.V2.MintDistributionCpv1 info) =>
        new(
            AmountFraction.From(info.BakingReward),
            AmountFraction.From(info.FinalizationReward)
        );
}

/// <summary>
/// The reward fractions related to the gas account and inclusion of special
/// transactions.
/// Introduce for protocol version 6.
/// </summary>
/// <param name="Baker">Fraction of the previous gas account paid to the baker.</param>
/// <param name="AccountCreation">Fraction paid for including each account creation transaction in a block.</param>
/// <param name="ChainUpdate">Fraction paid for including an update transaction in a block.</param>
public sealed record GasRewardsCpv2Update
    (AmountFraction Baker, AmountFraction AccountCreation, AmountFraction ChainUpdate) : IUpdatePayload
{
    internal static GasRewardsCpv2Update From(Grpc.V2.GasRewardsCpv2 update) =>
        new(
            AmountFraction.From(update.Baker),
            AmountFraction.From(update.AccountCreation),
            AmountFraction.From(update.ChainUpdate)
        );
}

/// <summary>
/// The consensus timeouts were updated (chain parameters version 2).
/// </summary>
public sealed record TimeoutParametersUpdate(TimeoutParameters TimeoutParameters) : IUpdatePayload
{
    internal static TimeoutParametersUpdate From(Grpc.V2.TimeoutParameters parameters) => new(TimeoutParameters.From(parameters));
}

/// <summary>
/// The minimum time between blocks was updated (chain parameters version 2).
/// </summary>
/// <param name="Duration">Min time between blocks.</param>
public sealed record MinBlockTimeUpdate(TimeSpan Duration) : IUpdatePayload
{
    internal static MinBlockTimeUpdate From(Grpc.V2.Duration duration) => new(TimeSpan.FromMilliseconds(duration.Value));
}

/// <summary>
/// The block energy limit was updated (chain parameters version 2).
/// </summary>
/// <param name="EnergyLimit">New Energy limit</param>
public sealed record BlockEnergyLimitUpdate(EnergyAmount EnergyLimit) : IUpdatePayload
{
    internal static BlockEnergyLimitUpdate From(Grpc.V2.Energy energy) => new(EnergyAmount.From(energy));
}

/// <summary>
/// Finalization committee parameters (chain parameters version 2).
/// </summary>
public sealed record FinalizationCommitteeParametersUpdate
    (FinalizationCommitteeParameters FinalizationCommitteeParameters) : IUpdatePayload
{
    internal static FinalizationCommitteeParametersUpdate From(Grpc.V2.FinalizationCommitteeParameters parameters) => new(FinalizationCommitteeParameters.From(parameters));
}

