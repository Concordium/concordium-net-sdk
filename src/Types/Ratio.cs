namespace Concordium.Sdk.Types;

/// <summary>
/// A ratio between two `ulong` integers.
///
/// It should be safe to assume the denominator is not zero and that numerator
/// and denominator are coprime.
/// </summary>
/// <param name="Numerator"></param>
/// <param name="Denominator"></param>
public record struct Ratio(ulong Numerator, ulong Denominator)
{
    internal static Ratio From(Grpc.V2.Ratio ratio) => new(ratio.Numerator, ratio.Denominator);
}
