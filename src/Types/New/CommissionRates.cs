namespace Concordium.Sdk.Types.New;

public record CommissionRates(
    decimal TransactionCommission,
    decimal FinalizationCommission,
    decimal BakingCommission)
{
    public static CommissionRates From(Grpc.V2.CommissionRates commissionRates)
    {
        throw new NotImplementedException();
    }
}
