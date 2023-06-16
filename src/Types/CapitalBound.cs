namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// A bound on the relative share of the total staked capital that a baker can
/// have as its stake. This is required to be greater than 0.
/// </summary>
public record CapitalBound(AmountFraction Bound)
{
    internal static CapitalBound From(Grpc.V2.CapitalBound capitalBound) => new(AmountFraction.From(capitalBound.Value));
}
