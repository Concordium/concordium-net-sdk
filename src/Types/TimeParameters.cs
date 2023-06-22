namespace Concordium.Sdk.Types;

/// <summary>
/// The time parameters are introduced as of protocol version 4, and consist of
/// the reward period length and the mint rate per payday. These are coupled as
/// a change to either affects the overall rate of minting.
/// </summary>
public sealed record TimeParameters(RewardPeriodLength RewardPeriodLength, MintRate MintPrPayDay)
{
    internal static TimeParameters From(Grpc.V2.TimeParametersCpv1 timeParams) =>
        new(RewardPeriodLength.From(timeParams.RewardPeriodLength),
            MintRate.From(timeParams.MintPerPayday));
}
