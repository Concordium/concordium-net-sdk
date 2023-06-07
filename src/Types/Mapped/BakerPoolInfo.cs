namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Additional information about a baking pool.
/// This information is added with the introduction of delegation in protocol
/// version 4.
/// </summary>
public class BakerPoolInfo
{
    /// <summary>
    /// The commission rates charged by the pool owner.
    /// </summary>
    public CommissionRates CommissionRates { get; init; }
    /// <summary>
    /// Whether the pool allows delegators.
    /// </summary>
    public BakerPoolOpenStatus OpenStatus { get; init; }
    /// <summary>
    /// The URL that links to the metadata about the pool.
    /// </summary>
    public string MetadataUrl { get; init; }

    private BakerPoolInfo() { }

    internal static BakerPoolInfo From(Concordium.Grpc.V2.BakerPoolInfo poolInfo) =>
        new()
        {
            CommissionRates = CommissionRates.From(poolInfo.CommissionRates),
            OpenStatus = BakerPoolOpenStatus.OpenForAll,
            MetadataUrl = poolInfo.Url
        };
}
