namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// An exchange rate between two quantities. This is never 0, and the exchange
/// rate should also never be infinite.
/// </summary>
public record struct ExchangeRate(ulong Numerator, ulong Denominator)
{
    internal static ExchangeRate From(Grpc.V2.ExchangeRate rate) => new(rate.Value.Numerator, rate.Value.Denominator);
}
