namespace Concordium.Sdk.Types;

/// <summary>
/// Ranges of allowed commission values that pools may choose from.
/// </summary>
/// <param name="Finalization">The range of allowed finalization commissions.</param>
/// <param name="Baking">The range of allowed baker commissions.</param>
/// <param name="Transaction">The range of allowed transaction commissions.</param>
public sealed record CommissionRanges(
    InclusiveRange<AmountFraction> Finalization,
    InclusiveRange<AmountFraction> Baking,
    InclusiveRange<AmountFraction> Transaction
)
{
    internal static CommissionRanges From(Grpc.V2.CommissionRanges ranges) =>
        new(
            InclusiveRange<AmountFraction>.From(ranges.Finalization),
            InclusiveRange<AmountFraction>.From(ranges.Baking),
            InclusiveRange<AmountFraction>.From(ranges.Transaction)
        );
}
