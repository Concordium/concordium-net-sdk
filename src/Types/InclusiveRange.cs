namespace Concordium.Sdk.Types;

/// <summary>
/// Range where <see cref="InclusiveRange{T}.Min"/> and <see cref="InclusiveRange{T}.Max"/> is within.
/// </summary>
/// <param name="Min">Min bound</param>
/// <param name="Max">Max bound</param>
/// <typeparam name="T"></typeparam>
public sealed record InclusiveRange<T>(T Min, T Max)
{
    internal static InclusiveRange<AmountFraction> From(Grpc.V2.InclusiveRangeAmountFraction range) =>
        new InclusiveRange<AmountFraction>(
            AmountFraction.From(range.Min),
            AmountFraction.From(range.Max));
}
