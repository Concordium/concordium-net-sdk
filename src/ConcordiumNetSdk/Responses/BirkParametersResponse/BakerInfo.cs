using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.BirkParametersResponse;

/// <summary>
/// Represents the information about a baker info.
/// </summary>
public record BakerInfo
{
    /// <summary>
    /// Gets or initiates the unique id of the baker.
    /// </summary>
    public int BakerId { get; init; }

    /// <summary>
    /// Gets or initiates the address of the account to which the baker gets their reward.
    /// </summary>
    public AccountAddress BakerAccount { get; init; }

    /// <summary>
    /// Gets or initiates the baker's current lottery power. At the moment this is still fixed at genesis,
    /// but in the future the lottery power will be calculated based on their stake.
    /// </summary>
    public decimal BakerLotteryPower { get; init; }
}
