namespace Concordium.Sdk.Types;

/// <summary>
/// Finalization committee parameters. These parameters control which bakers are
/// in the finalization committee.
/// </summary>
/// <param name="MinFinalizers">
/// Minimum number of bakers to include in the finalization committee before
/// the <see cref="FinalizationCommitteeParameters.FinalizersRelativeStakeThreshold"/> takes effect.
/// </param>
/// <param name="MaxFinalizers">Maximum number of bakers to include in the finalization committee.</param>
/// <param name="FinalizersRelativeStakeThreshold">
/// Determining the staking threshold required for being eligible the
/// finalization committee. The required amount is given by `total stake
/// in pools * <see cref="FinalizationCommitteeParameters.FinalizersRelativeStakeThreshold"/>`.
/// Accepted values are between a value of 0 and 1.
/// </param>
public record FinalizationCommitteeParameters(uint MinFinalizers, uint MaxFinalizers,
    AmountFraction FinalizersRelativeStakeThreshold)
{
    internal static FinalizationCommitteeParameters From(Grpc.V2.FinalizationCommitteeParameters committee) =>
        new(
            committee.MinimumFinalizers,
            committee.MaximumFinalizers,
            AmountFraction.From(committee.FinalizerRelativeStakeThreshold)
        );
}
