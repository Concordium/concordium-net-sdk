using Concordium.Sdk.Types.Mapped;

namespace Concordium.Sdk.Types.New;

public record ChainParametersV0(
    decimal ElectionDifficulty,
    ExchangeRate EuroPerEnergy,
    ExchangeRate MicroGTUPerEuro,
    ulong BakerCooldownEpochs,
    ushort AccountCreationLimit,
    RewardParametersV0 RewardParameters,
    ulong FoundationAccountIndex,
    CcdAmount MinimumThresholdForBaking);