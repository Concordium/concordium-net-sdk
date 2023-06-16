namespace Concordium.Sdk.Types;

/// <summary>
/// The amount of leverage that a baker can get from delegation. A leverage
/// factor of 1 means that a baker does not gain anything from delegation.
/// </summary>
public record struct LeverageFactor(ulong Numerator, ulong Denominator)
{
    internal static LeverageFactor From(Grpc.V2.LeverageFactor factor) => new(factor.Value.Numerator, factor.Value.Denominator);
}
