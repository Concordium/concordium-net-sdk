namespace Concordium.Sdk.Types;

/// <summary>
/// Mint distribution that applies to protocol versions 1-3 with chain parameters version 0.
/// </summary>
/// <param name="MintPerSlot">The increase in CCD amount per slot.</param>
/// <param name="BakingReward">Fraction of newly minted CCD allocated to baker rewards.</param>
/// <param name="FinalizationReward">Fraction of newly minted CCD allocated to finalization rewards.</param>
public sealed record MintDistributionCpv0(MintRate MintPerSlot, AmountFraction BakingReward, AmountFraction FinalizationReward)
{
    internal static MintDistributionCpv0 From(Grpc.V2.MintDistributionCpv0 chainParameterVersion0) =>
        new(
            MintRate.From(chainParameterVersion0.MintPerSlot),
            AmountFraction.From(chainParameterVersion0.BakingReward),
            AmountFraction.From(chainParameterVersion0.FinalizationReward)
        );
}

/// <summary>
/// Mint distribution parameters that apply to protocol version 4 and up.
/// </summary>
/// <param name="BakingReward">Fraction of newly minted CCD allocated to baker rewards.</param>
/// <param name="FinalizationReward">Fraction of newly minted CCD allocated to finalization rewards.</param>
public sealed record MintDistributionCpv1(AmountFraction BakingReward, AmountFraction FinalizationReward)
{
    internal static MintDistributionCpv1 From(Grpc.V2.MintDistributionCpv1 chainParameterVersion1) =>
        new(
            AmountFraction.From(chainParameterVersion1.BakingReward),
            AmountFraction.From(chainParameterVersion1.FinalizationReward)
        );
}
