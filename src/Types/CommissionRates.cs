namespace Concordium.Sdk.Types;

/// <summary>
/// The commission rates charged by the pool owner.
/// </summary>
/// <param name="TransactionCommission">Fraction of transaction rewards charged by the pool owner.</param>
/// <param name="FinalizationCommission">Fraction of finalization rewards charged by the pool owner.</param>
/// <param name="BakingCommission">Fraction of baking rewards charged by the pool owner.</param>
public sealed record CommissionRates(
    AmountFraction TransactionCommission,
    AmountFraction FinalizationCommission,
    AmountFraction BakingCommission)
{
    internal static CommissionRates From(Grpc.V2.CommissionRates commissionRates) =>
        new(
            AmountFraction.From(commissionRates.Transaction),
            AmountFraction.From(commissionRates.Finalization),
            AmountFraction.From(commissionRates.Baking)
        );
}
