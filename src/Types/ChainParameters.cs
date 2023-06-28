using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Chain parameters.
/// </summary>
public interface IChainParameters
{}

internal static class ChainParametersFactory
{
    internal static IChainParameters From(Grpc.V2.ChainParameters chainParameters) =>
        chainParameters.ParametersCase switch
        {
            ChainParameters.ParametersOneofCase.V0 =>
                ChainParametersV0.From(chainParameters.V0),
            ChainParameters.ParametersOneofCase.V1 =>
                ChainParametersV1.From(chainParameters.V1),
            ChainParameters.ParametersOneofCase.V2 =>
                ChainParametersV2.From(chainParameters.V2),
            ChainParameters.ParametersOneofCase.None =>
                throw new MissingEnumException<ChainParameters.ParametersOneofCase>(chainParameters.ParametersCase),
            _ => throw new MissingEnumException<ChainParameters.ParametersOneofCase>(chainParameters.ParametersCase)
        };
}

/// <summary>
/// Values of chain parameters that can be updated via chain updates.
/// This applies to protocol version 6 and up.
/// </summary>
/// <param name="TimeoutParameters">Consensus protocol version 2 timeout parameters.</param>
/// <param name="MinBlockTime">Minimum time interval between blocks.</param>
/// <param name="BlockEnergyLimit">Maximum energy allowed per block.</param>
/// <param name="EuroPerEnergy">Euro per energy exchange rate.</param>
/// <param name="MicroCcdPerEuro">Micro ccd per euro exchange rate.</param>
/// <param name="CooldownParameters">Parameters related to cooldowns when staking.</param>
/// <param name="TimeParameters">Parameters related mint rate and reward period.</param>
/// <param name="AccountCreationLimit">The limit for the number of account creations in a block.</param>
/// <param name="MintDistribution">Parameters related to the distribution of newly minted CCD.</param>
/// <param name="TransactionFeeDistribution">Parameters related to the distribution of transaction fees.</param>
/// <param name="GasRewards">Parameters related to the distribution from the GAS account.</param>
/// <param name="FoundationAccount">Address of the foundation account.</param>
/// <param name="PoolParameters">Parameters for baker pools.</param>
/// <param name="FinalizationCommitteeParameters">The finalization committee parameters.</param>
/// <param name="RootKeys">Root Keys</param>
/// <param name="Level1Keys">Level 1 Keys</param>
/// <param name="Level2Keys">Level 2 Keys</param>
public sealed record ChainParametersV2(
    TimeoutParameters TimeoutParameters,
    TimeSpan MinBlockTime,
    EnergyAmount BlockEnergyLimit,
    ExchangeRate EuroPerEnergy,
    ExchangeRate MicroCcdPerEuro,
    CooldownParameters CooldownParameters,
    TimeParameters TimeParameters,
    CredentialsPerBlockLimit AccountCreationLimit,
    MintDistributionCpv1 MintDistribution,
    TransactionFeeDistribution TransactionFeeDistribution,
    GasRewardsCpv2 GasRewards,
    AccountAddress FoundationAccount,
    PoolParameters PoolParameters,
    FinalizationCommitteeParameters FinalizationCommitteeParameters,
    HigherLevelKeys RootKeys,
    HigherLevelKeys Level1Keys,
    AuthorizationsV1 Level2Keys
) : IChainParameters
{
    internal static ChainParametersV2 From(Grpc.V2.ChainParametersV2 chainParametersV2) =>
        new(
            TimeoutParameters.From(chainParametersV2.ConsensusParameters.TimeoutParameters),
            TimeSpan.FromMilliseconds(chainParametersV2.ConsensusParameters.MinBlockTime.Value),
            EnergyAmount.From(chainParametersV2.ConsensusParameters.BlockEnergyLimit),
            ExchangeRate.From(chainParametersV2.EuroPerEnergy),
            ExchangeRate.From(chainParametersV2.MicroCcdPerEuro),
            CooldownParameters.From(chainParametersV2.CooldownParameters),
            TimeParameters.From(chainParametersV2.TimeParameters),
            CredentialsPerBlockLimit.From(chainParametersV2.AccountCreationLimit),
            MintDistributionCpv1.From(chainParametersV2.MintDistribution),
            TransactionFeeDistribution.From(chainParametersV2.TransactionFeeDistribution),
            GasRewardsCpv2.From(chainParametersV2.GasRewards),
            AccountAddress.From(chainParametersV2.FoundationAccount),
            PoolParameters.From(chainParametersV2.PoolParameters),
            FinalizationCommitteeParameters.From(chainParametersV2.FinalizationCommitteeParameters),
            Types.RootKeys.From(chainParametersV2.RootKeys),
            Types.Level1Keys.From(chainParametersV2.Level1Keys),
            AuthorizationsV1.From(chainParametersV2.Level2Keys)
        );
}

/// <summary>
/// Values of chain parameters that can be updated via chain updates.
/// This applies to protocol version 4 and 5.
/// </summary>
/// <param name="ElectionDifficulty">Election difficulty for consensus lottery.</param>
/// <param name="EuroPerEnergy">Euro per energy exchange rate.</param>
/// <param name="MicroCcdPerEuro">Micro ccd per euro exchange rate.</param>
/// <param name="CooldownParameters">Cooldown parameters.</param>
/// <param name="TimeParameters">Time parameters.</param>
/// <param name="AccountCreationLimit">The limit for the number of account creations in a block.</param>
/// <param name="MintDistribution">Parameters related to the distribution of newly minted CCD.</param>
/// <param name="TransactionFeeDistribution">Parameters related to the distribution of transaction fees.</param>
/// <param name="GasRewards">Parameters related to the distribution of the GAS account.</param>
/// <param name="FoundationAccount">Address of the foundation account.</param>
/// <param name="PoolParameters">Parameters for baker pools.</param>
/// <param name="RootKeys">Root Keys</param>
/// <param name="Level1Keys">Level 1 Keys</param>
/// <param name="Level2Keys">Level 2 Keys</param>
public sealed record ChainParametersV1(
    AmountFraction ElectionDifficulty,
    ExchangeRate EuroPerEnergy,
    ExchangeRate MicroCcdPerEuro,
    CooldownParameters CooldownParameters,
    TimeParameters TimeParameters,
    CredentialsPerBlockLimit AccountCreationLimit,
    MintDistributionCpv1 MintDistribution,
    TransactionFeeDistribution TransactionFeeDistribution,
    GasRewards GasRewards,
    AccountAddress FoundationAccount,
    PoolParameters PoolParameters,
    HigherLevelKeys RootKeys,
    HigherLevelKeys Level1Keys,
    AuthorizationsV1 Level2Keys
) : IChainParameters
{
    internal static ChainParametersV1 From(Grpc.V2.ChainParametersV1 chainParametersV1) =>
        new(
            AmountFraction.From(chainParametersV1.ElectionDifficulty.Value),
            ExchangeRate.From(chainParametersV1.EuroPerEnergy),
            ExchangeRate.From(chainParametersV1.MicroCcdPerEuro),
            CooldownParameters.From(chainParametersV1.CooldownParameters),
            TimeParameters.From(chainParametersV1.TimeParameters),
            CredentialsPerBlockLimit.From(chainParametersV1.AccountCreationLimit),
            MintDistributionCpv1.From(chainParametersV1.MintDistribution),
            TransactionFeeDistribution.From(chainParametersV1.TransactionFeeDistribution),
            GasRewards.From(chainParametersV1.GasRewards),
            AccountAddress.From(chainParametersV1.FoundationAccount),
            PoolParameters.From(chainParametersV1.PoolParameters),
            Types.RootKeys.From(chainParametersV1.RootKeys),
            Types.Level1Keys.From(chainParametersV1.Level1Keys),
            AuthorizationsV1.From(chainParametersV1.Level2Keys)
        );
}

/// <summary>
/// Values of chain parameters that can be updated via chain updates.
/// This applies to protocol version 1-3.
/// </summary>
/// <param name="ElectionDifficulty">Election difficulty for consensus lottery.</param>
/// <param name="EuroPerEnergy">Euro per energy exchange rate.</param>
/// <param name="MicroCcdPerEuro">Micro ccd per euro exchange rate.</param>
/// <param name="BakerCooldownEpochs">
/// Extra number of epochs before reduction in stake, or baker
/// deregistration is completed.
/// </param>
/// <param name="AccountCreationLimit">The limit for the number of account creations in a block.</param>
/// <param name="MintDistribution">Parameters related to the distribution of newly minted CCD.</param>
/// <param name="TransactionFeeDistribution">Parameters related to the distribution of transaction fees.</param>
/// <param name="GasRewards">Parameters related to the distribution of the GAS account.</param>
/// <param name="FoundationAccount"></param>
/// <param name="MinimumThresholdForBaking">Address of the foundation account.</param>
/// <param name="RootKeys">Root Keys</param>
/// <param name="Level1Keys">Level 1 Keys</param>
/// <param name="Level2Keys">Level 2 Keys</param>
public sealed record ChainParametersV0(
    AmountFraction ElectionDifficulty,
    ExchangeRate EuroPerEnergy,
    ExchangeRate MicroCcdPerEuro,
    Epoch BakerCooldownEpochs,
    CredentialsPerBlockLimit AccountCreationLimit,
    MintDistributionCpv0 MintDistribution,
    TransactionFeeDistribution TransactionFeeDistribution,
    GasRewards GasRewards,
    AccountAddress FoundationAccount,
    CcdAmount MinimumThresholdForBaking,
    RootKeys RootKeys,
    Level1Keys Level1Keys,
    AuthorizationsV0 Level2Keys
) : IChainParameters
{
    internal static ChainParametersV0 From(Grpc.V2.ChainParametersV0 chainParametersV0) =>
        new(
            AmountFraction.From(chainParametersV0.ElectionDifficulty.Value),
            ExchangeRate.From(chainParametersV0.EuroPerEnergy),
            ExchangeRate.From(chainParametersV0.MicroCcdPerEuro),
            Epoch.From(chainParametersV0.BakerCooldownEpochs),
            CredentialsPerBlockLimit.From(chainParametersV0.AccountCreationLimit),
            MintDistributionCpv0.From(chainParametersV0.MintDistribution),
            TransactionFeeDistribution.From(chainParametersV0.TransactionFeeDistribution),
            GasRewards.From(chainParametersV0.GasRewards),
            AccountAddress.From(chainParametersV0.FoundationAccount),
            chainParametersV0.MinimumThresholdForBaking.ToCcd(),
            Types.RootKeys.From(chainParametersV0.RootKeys),
            Types.Level1Keys.From(chainParametersV0.Level1Keys),
            AuthorizationsV0.From(chainParametersV0.Level2Keys));
}
