namespace Concordium.Sdk.Types.New;

public class BakerPoolInfo
{

    public CommissionRates CommissionRates { get; init; }
    public BakerPoolOpenStatus OpenStatus { get; init; }
    public string MetadataUrl { get; init; }

    public BakerPoolInfo(CommissionRates commissionRates,
        BakerPoolOpenStatus openStatus,
        string metadataUrl)
    {
        this.CommissionRates = commissionRates;
        this.OpenStatus = openStatus;
        this.MetadataUrl = metadataUrl;
    }

    internal static BakerPoolInfo From(Concordium.Grpc.V2.BakerPoolInfo poolInfo)
    {
        throw new NotImplementedException();
    }
}
