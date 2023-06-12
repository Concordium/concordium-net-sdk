using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// The commission rates charged by the pool owner.
/// </summary>
/// <param name="TransactionCommission">Fraction of transaction rewards charged by the pool owner.</param>
/// <param name="FinalizationCommission">Fraction of finalization rewards charged by the pool owner.</param>
/// <param name="BakingCommission">Fraction of baking rewards charged by the pool owner.</param>
public record CommissionRates(
    decimal TransactionCommission,
    decimal FinalizationCommission,
    decimal BakingCommission)
{
    private const decimal MultiplicationFactor = 1 / 100_000m;
    internal static CommissionRates From(Grpc.V2.CommissionRates commissionRates) =>
        new(
            ToDecimal(commissionRates.Transaction),
            ToDecimal(commissionRates.Finalization),
            ToDecimal(commissionRates.Baking)
        );
    private static decimal ToDecimal(AmountFraction amountFraction) => amountFraction.PartsPerHundredThousand * MultiplicationFactor;
}
