using Concordium.Sdk.Exceptions;
using GrpcEffect = Concordium.Grpc.V2.PendingUpdate.EffectOneofCase;

namespace Concordium.Sdk.Types;
/// <summary>
/// Minimum stake needed to become a baker. This only applies to protocol version 1-3.
/// </summary>
/// <param name="MinimumThresholdForBaking">Minimum threshold required for registering as a baker.</param>
public record BakerStakeThreshold(CcdAmount MinimumThresholdForBaking)
{
    internal static BakerStakeThreshold From(Grpc.V2.BakerStakeThreshold bakerStakeThreshold) => new(CcdAmount.From(bakerStakeThreshold.BakerStakeThreshold_));
};

/// <summary>
/// A pending update.
/// </summary>
/// <param name="EffectiveTime">The effective time of the update.</param>
/// <param name="Effect">The effect of the update.</param>
public sealed record PendingUpdate(TransactionTime EffectiveTime, IEffect Effect)
{
    internal static PendingUpdate From(Grpc.V2.PendingUpdate pendingUpdate) =>
        new(
            TransactionTime.From(pendingUpdate.EffectiveTime),
            pendingUpdate.EffectCase switch
            {
                GrpcEffect.RootKeys => new EffectRootKeys(RootKeys.From(pendingUpdate.RootKeys)),
                GrpcEffect.Level1Keys => new EffectLevel1Keys(Level1Keys.From(pendingUpdate.Level1Keys)),
                GrpcEffect.Level2KeysCpv0 => new EffectLevel2KeysCpv0(AuthorizationsV0.From(pendingUpdate.Level2KeysCpv0)),
                GrpcEffect.Level2KeysCpv1 => new EffectLevel2KeysCpv1(AuthorizationsV1.From(pendingUpdate.Level2KeysCpv1)),
                GrpcEffect.Protocol => new EffectProtocol(ProtocolUpdate.From(pendingUpdate.Protocol)),
                GrpcEffect.ElectionDifficulty => new EffectElectionDifficulty(ElectionDifficulty.From(pendingUpdate.ElectionDifficulty)),
                GrpcEffect.EuroPerEnergy => new EffectEuroPerEnergy(ExchangeRate.From(pendingUpdate.EuroPerEnergy)),
                GrpcEffect.MicroCcdPerEuro => new EffectMicroCcdPerEnergy(ExchangeRate.From(pendingUpdate.MicroCcdPerEuro)),
                GrpcEffect.FoundationAccount => new EffectFoundationAccount(AccountAddress.From(pendingUpdate.FoundationAccount)),
                GrpcEffect.MintDistributionCpv0 => new EffectMintDistributionCpv0(MintDistributionCpv0.From(pendingUpdate.MintDistributionCpv0)),
                GrpcEffect.MintDistributionCpv1 => new EffectMintDistributionCpv1(MintDistributionCpv1.From(pendingUpdate.MintDistributionCpv1)),
                GrpcEffect.TransactionFeeDistribution => new EffectTransactionFeeDistribution(TransactionFeeDistribution.From(pendingUpdate.TransactionFeeDistribution)),
                GrpcEffect.GasRewards => new EffectGasRewards(GasRewards.From(pendingUpdate.GasRewards)),
                GrpcEffect.PoolParametersCpv0 => new EffectPoolParametersCpv0(BakerStakeThreshold.From(pendingUpdate.PoolParametersCpv0)),
                GrpcEffect.PoolParametersCpv1 => new EffectPoolParametersCpv1(PoolParameters.From(pendingUpdate.PoolParametersCpv1)),
                GrpcEffect.AddAnonymityRevoker => new EffectAddAnonymityRevoker(ArInfo.From(pendingUpdate.AddAnonymityRevoker)),
                GrpcEffect.AddIdentityProvider => new EffectAddIdentityProvider(IpInfo.From(pendingUpdate.AddIdentityProvider)),
                GrpcEffect.CooldownParameters => new EffectCooldownParameters(CooldownParameters.From(pendingUpdate.CooldownParameters)),
                GrpcEffect.TimeParameters => new EffectTimeParameters(TimeParameters.From(pendingUpdate.TimeParameters)),
                GrpcEffect.GasRewardsCpv2 => new EffectGasRewardsCpv2(GasRewardsCpv2.From(pendingUpdate.GasRewardsCpv2)),
                GrpcEffect.TimeoutParameters => new EffectTimeoutParameters(TimeoutParameters.From(pendingUpdate.TimeoutParameters)),
                GrpcEffect.MinBlockTime => new EffectMinBlockTime(TimeSpan.FromMilliseconds(pendingUpdate.MinBlockTime.Value)),
                GrpcEffect.BlockEnergyLimit => new EffectBlockEnergyLimit(EnergyAmount.From(pendingUpdate.BlockEnergyLimit)),
                GrpcEffect.FinalizationCommitteeParameters => new EffectFinalizationCommitteeParameters(FinalizationCommitteeParameters.From(pendingUpdate.FinalizationCommitteeParameters)),
                GrpcEffect.None => throw new NotImplementedException(),
                _ => throw new MissingEnumException<GrpcEffect>(pendingUpdate.EffectCase),
            }
        );
}

/// <summary>The effect of the update.</summary>
public interface IEffect { };

/// <summary>Updates to the root keys.</summary>
public sealed record EffectRootKeys(RootKeys RootKeys) : IEffect;
/// <summary>Updates to the level 1 keys.</summary>
public sealed record EffectLevel1Keys(Level1Keys Level1Keys) : IEffect;
/// <summary>Updates to the level 2 keys.</summary>
public sealed record EffectLevel2KeysCpv0(AuthorizationsV0 Level2KeysUpdateV0) : IEffect;
/// <summary>Updates to the level 2 keys.</summary>
public sealed record EffectLevel2KeysCpv1(AuthorizationsV1 Level2KeysUpdateV1) : IEffect;
/// <summary>Protocol updates.</summary>
public sealed record EffectProtocol(ProtocolUpdate ProtocolUpdate) : IEffect;
/// <summary>Updates to the election difficulty parameter.</summary>
public sealed record EffectElectionDifficulty(ElectionDifficulty ElectionDifficulty) : IEffect;
/// <summary>Updates to the euro:energy exchange rate.</summary>
public sealed record EffectEuroPerEnergy(ExchangeRate EuroPerEnergy) : IEffect;
/// <summary>Updates to the CCD:EUR exchange rate.</summary>
public sealed record EffectMicroCcdPerEnergy(ExchangeRate MicroCcdPerEnergy) : IEffect;
/// <summary>Updates to the foundation account.</summary>
public sealed record EffectFoundationAccount(AccountAddress FoundationAccount) : IEffect;
/// <summary>Updates to the mint distribution. Is only relevant prior to protocol version 4.</summary>
public sealed record EffectMintDistributionCpv0(MintDistributionCpv0 MintDistributionCpv0) : IEffect;
/// <summary>The mint distribution was updated. Introduced in protocol version 4.</summary>
public sealed record EffectMintDistributionCpv1(MintDistributionCpv1 MintDistributionCpv1) : IEffect;
/// <summary>Updates to the transaction fee distribution.</summary>
public sealed record EffectTransactionFeeDistribution(TransactionFeeDistribution TransactionFeeDistribution) : IEffect;
/// <summary>Updates to the GAS rewards.</summary>
public sealed record EffectGasRewards(GasRewards GasRewards) : IEffect;
/// <summary>Updates baker stake threshold. Is only relevant prior to protocol version 4.</summary>
public sealed record EffectPoolParametersCpv0(BakerStakeThreshold BakerParameters) : IEffect;
/// <summary>Updates pool parameters. Introduced in protocol version 4.</summary>
public sealed record EffectPoolParametersCpv1(PoolParameters PoolParameters) : IEffect;
/// <summary>Adds a new anonymity revoker.</summary>
public sealed record EffectAddAnonymityRevoker(ArInfo AddAnonymityRevoker) : IEffect;
/// <summary>Adds a new identity provider.</summary>
public sealed record EffectAddIdentityProvider(IpInfo AddIdentityProvider) : IEffect;
/// <summary>Updates to cooldown parameters for chain parameters version 1 introduced in protocol version 4.</summary>
public sealed record EffectCooldownParameters(CooldownParameters CooldownParameters) : IEffect;
/// <summary>Updates to time parameters for chain parameters version 1 introduced in protocol version 4.</summary>
public sealed record EffectTimeParameters(TimeParameters TimeParameters) : IEffect;
/// <summary>Updates to the GAS rewards effective from protocol version 6 (chain parameters version 2).</summary>
public sealed record EffectGasRewardsCpv2(GasRewardsCpv2 GasRewardsCpv2) : IEffect;
/// <summary>Updates to the consensus timeouts for chain parameters version 2.</summary>
public sealed record EffectTimeoutParameters(TimeoutParameters TimeoutParameters) : IEffect;
/// <summary>Updates to the the minimum time between blocks for chain parameters version 2.</summary>
public sealed record EffectMinBlockTime(TimeSpan MinBlockTime) : IEffect;
/// <summary>Updates to the block energy limit for chain parameters version 2.</summary>
public sealed record EffectBlockEnergyLimit(EnergyAmount BlockEnergyLimit) : IEffect;
/// <summary>Updates to the finalization committee for for chain parameters version 2.</summary>
public sealed record EffectFinalizationCommitteeParameters(FinalizationCommitteeParameters FinalizationCommitteeParameters) : IEffect;
