using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a chain parameters version 0.
/// </summary>
public record ChainParametersV0 : ChainParameters
{
    /// <summary>
    /// Gets or initiates the reward parameters.
    /// </summary>
    public RewardParametersV0 RewardParameters { get; init; }

    //todo: think of implementing epoch class
    /// <summary>
    /// Gets or initiates the baker cooldown epochs.
    /// </summary>
    public ulong BakerCooldownEpochs { get; init; }

    /// <summary>
    /// Gets or initiates the minimum threshold for baking.
    /// </summary>
    public CcdAmount MinimumThresholdForBaking { get; init; }
}
