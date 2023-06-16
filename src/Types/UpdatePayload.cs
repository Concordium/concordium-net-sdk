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

internal static class UpdatePayloadFactory
{
    internal static IUpdatePayload From(Grpc.V2.UpdatePayload payload) =>
        payload.PayloadCase switch
        {
            UpdatePayload.PayloadOneofCase.ProtocolUpdate =>
                Protocol.From(payload.ProtocolUpdate),
            UpdatePayload.PayloadOneofCase.ElectionDifficultyUpdate =>
                ElectionDifficulty.From(payload.ElectionDifficultyUpdate),
            UpdatePayload.PayloadOneofCase.EuroPerEnergyUpdate =>
                EuroPerEnergy.From(payload.EuroPerEnergyUpdate),
            UpdatePayload.PayloadOneofCase.MicroCcdPerEuroUpdate =>
                MicroCcdPerEuro.From(payload.MicroCcdPerEuroUpdate),
            UpdatePayload.PayloadOneofCase.FoundationAccountUpdate =>
                FoundationAccount.From(payload.FoundationAccountUpdate),
            UpdatePayload.PayloadOneofCase.MintDistributionUpdate =>
                MintDistributionCpv0.From(payload.MintDistributionUpdate),
            UpdatePayload.PayloadOneofCase.TransactionFeeDistributionUpdate =>
                TransactionFeeDistribution.From(payload.TransactionFeeDistributionUpdate),
            UpdatePayload.PayloadOneofCase.GasRewardsUpdate =>
                GasRewards.From(payload.GasRewardsUpdate),
            UpdatePayload.PayloadOneofCase.BakerStakeThresholdUpdate =>
                BakerStakeThreshold.From(payload.BakerStakeThresholdUpdate),
            UpdatePayload.PayloadOneofCase.RootUpdate =>
                Root.From(payload.RootUpdate),
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
                TimeParametersCpv1.From(payload.TimeParametersCpv1Update),
            UpdatePayload.PayloadOneofCase.MintDistributionCpv1Update =>
                MintDistributionCpv1.From(payload.MintDistributionCpv1Update),
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
public record Protocol(string Message, string SpecificationUrl, Sha256Hash SpecificationHash, byte[] SpecificationAuxiliaryData) : IUpdatePayload
{
    internal static Protocol From(Grpc.V2.ProtocolUpdate update) =>
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
public record ElectionDifficulty(AmountFraction PartsPerHundredThousands) : IUpdatePayload
{
    internal static ElectionDifficulty From(Grpc.V2.ElectionDifficulty fraction) =>
        new(AmountFraction.From(fraction.Value));
}

/// <summary>
/// Exchange rate for euro pr. energy.
/// </summary>
/// <param name="ExchangeRate"></param>
public record EuroPerEnergy(ExchangeRate ExchangeRate) : IUpdatePayload
{
    internal static EuroPerEnergy From(Grpc.V2.ExchangeRate rate) => new(ExchangeRate.From(rate));
}

/// <summary>
/// Exchange rate for micro ccd pr. euro.
/// </summary>
/// <param name="ExchangeRate"></param>
public record MicroCcdPerEuro(ExchangeRate ExchangeRate) : IUpdatePayload
{
    internal static MicroCcdPerEuro From(Grpc.V2.ExchangeRate rate) => new(ExchangeRate.From(rate));
}

/// <summary>
/// Update of the foundation account.
/// </summary>
public record FoundationAccount(AccountAddress AccountAddress) : IUpdatePayload
{
    internal static FoundationAccount From(Grpc.V2.AccountAddress address) => new(AccountAddress.From(address));
}

/// <summary>
/// Mint distribution that applies to protocol versions 1-3 with chain parameters version 0.
/// </summary>
/// <param name="MintPerSlot">The increase in CCD amount per slot.</param>
/// <param name="BakingReward">Fraction of newly minted CCD allocated to baker rewards.</param>
/// <param name="FinalizationReward">Fraction of newly minted CCD allocated to finalization rewards.</param>
public record MintDistributionCpv0(MintRate MintPerSlot, AmountFraction BakingReward, AmountFraction FinalizationReward) : IUpdatePayload
{
    internal static MintDistributionCpv0 From(Grpc.V2.MintDistributionCpv0 chainParameterVersion0) =>
        new(
            MintRate.From(chainParameterVersion0.MintPerSlot),
            AmountFraction.From(chainParameterVersion0.BakingReward),
            AmountFraction.From(chainParameterVersion0.FinalizationReward)
        );
}

/// <summary>
/// Update the transaction fee distribution to the specified value.
/// </summary>
/// <param name="Baker">The fraction that goes to the baker of the block.</param>
/// <param name="GasAccount">
/// The fraction that goes to the gas account. The remaining fraction will
/// go to the foundation.
/// </param>
public record TransactionFeeDistribution(AmountFraction Baker, AmountFraction GasAccount) : IUpdatePayload
{
    internal static TransactionFeeDistribution From(Grpc.V2.TransactionFeeDistribution distribution) =>
        new(
            AmountFraction.From(distribution.Baker),
            AmountFraction.From(distribution.GasAccount)
        );
}

/// <summary>
/// The reward fractions related to the gas account and inclusion of special
/// transactions.
/// </summary>
/// <param name="Baker">Fraction of the previous gas account paid to the baker.</param>
/// <param name="FinalizationProof">Fraction paid for including a finalization proof in a block.</param>
/// <param name="AccountCreation">Fraction paid for including each account creation transaction in a block.</param>
/// <param name="ChainUpdate">Fraction paid for including an update transaction in a block.</param>
public record GasRewards(AmountFraction Baker, AmountFraction FinalizationProof, AmountFraction AccountCreation, AmountFraction ChainUpdate) : IUpdatePayload
{
    internal static GasRewards From(Grpc.V2.GasRewards rewards) =>
        new(
            AmountFraction.From(rewards.Baker),
            AmountFraction.From(rewards.FinalizationProof),
            AmountFraction.From(rewards.AccountCreation),
            AmountFraction.From(rewards.ChainUpdate)
        );
}

/// <summary>
/// Parameters related to becoming a baker that apply to protocol versions 1-3.
/// </summary>
/// <param name="MinimumThresholdForBaking">Minimum amount of CCD that an account must stake to become a baker.</param>
public record BakerStakeThreshold(CcdAmount MinimumThresholdForBaking) : IUpdatePayload
{
    internal static BakerStakeThreshold From(Grpc.V2.BakerStakeThreshold threshold) =>
        new(threshold.BakerStakeThreshold_.ToCcd());
}

/// <summary>
/// An update with root keys of some other set of governance keys, or the root
/// keys themselves.
/// </summary>
public record Root(IRootUpdate RootUpdate) : IUpdatePayload
{
    internal static Root From(Grpc.V2.RootUpdate root) => new(RootUpdateFactory.From(root));
}

/// <summary>
/// Update to anonymity revoker.
/// </summary>
public record AddAnonymityRevokerUpdate(ArInfo ArInfo) : IUpdatePayload
{
    internal static AddAnonymityRevokerUpdate From(Grpc.V2.ArInfo info) =>
        new(ArInfo.From(info));
}

/// <summary>
/// Update to identity provider.
/// </summary>
public record AddIdentityProviderUpdate(IpInfo IpInfo) : IUpdatePayload
{
    internal static AddIdentityProviderUpdate From(Grpc.V2.IpInfo info) => new(IpInfo.From(info));
}

/// <summary>
/// Update to cooldown parameters with chain parameter version 1.
/// </summary>
public record CooldownParametersCpv1Update(CooldownParameters CooldownParameters) : IUpdatePayload
{
    internal static CooldownParametersCpv1Update From(Grpc.V2.CooldownParametersCpv1 cooldown) =>
        new(CooldownParameters.From(cooldown));
}

/// <summary>
/// Update to pool- and delegation parameters.
/// </summary>
public record PoolParametersCpv1Update(PoolParameters PoolParameters) : IUpdatePayload
{
    internal static PoolParametersCpv1Update From(Grpc.V2.PoolParametersCpv1 pool) =>
        new(PoolParameters.From(pool));
}

/// <summary>
/// Update to mint rate pr day and reward period length.
/// </summary>
/// <param name="TimeParameters"></param>
public record TimeParametersCpv1(TimeParameters TimeParameters) : IUpdatePayload
{
    internal static TimeParametersCpv1 From(Grpc.V2.TimeParametersCpv1 timeParametersCpv1) => new(TimeParameters.From(timeParametersCpv1));
}

/// <summary>
/// Mint distribution parameters that apply to protocol version 4 and up. with chain parameters version 1.
/// </summary>
/// <param name="BakingReward">Fraction of newly minted CCD allocated to baker rewards.</param>
/// <param name="FinalizationReward">Fraction of newly minted CCD allocated to finalization rewards.</param>
public record MintDistributionCpv1(AmountFraction BakingReward, AmountFraction FinalizationReward) : IUpdatePayload
{
    internal static MintDistributionCpv1 From(Grpc.V2.MintDistributionCpv1 info) =>
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
public record GasRewardsCpv2Update
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
public record TimeoutParametersUpdate(TimeoutParameters TimeoutParameters) : IUpdatePayload
{
    internal static TimeoutParametersUpdate From(Grpc.V2.TimeoutParameters parameters) => new(TimeoutParameters.From(parameters));
}

/// <summary>
/// The minimum time between blocks was updated (chain parameters version 2).
/// </summary>
/// <param name="Duration">Min time between blocks.</param>
public record MinBlockTimeUpdate(TimeSpan Duration) : IUpdatePayload
{
    internal static MinBlockTimeUpdate From(Grpc.V2.Duration duration) => new(TimeSpan.FromMilliseconds(duration.Value));
}

/// <summary>
/// The block energy limit was updated (chain parameters version 2).
/// </summary>
/// <param name="EnergyLimit">New Energy limit</param>
public record BlockEnergyLimitUpdate(EnergyAmount EnergyLimit) : IUpdatePayload
{
    internal static BlockEnergyLimitUpdate From(Grpc.V2.Energy energy) => new(EnergyAmount.From(energy));
}

/// <summary>
/// Finalization committee parameters (chain parameters version 2).
/// </summary>
public record FinalizationCommitteeParametersUpdate
    (FinalizationCommitteeParameters FinalizationCommitteeParameters) : IUpdatePayload
{
    internal static FinalizationCommitteeParametersUpdate From(Grpc.V2.FinalizationCommitteeParameters parameters) => new(FinalizationCommitteeParameters.From(parameters));
}

