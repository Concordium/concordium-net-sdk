namespace Concordium.Sdk.Types;

/// <summary>
/// Additional information about a baking pool.
/// This information is added with the introduction of delegation in protocol
/// version 4.
/// </summary>
/// <param name="CommissionRates">The commission rates charged by the pool owner.</param>
/// <param name="OpenStatus">Whether the pool allows delegators.</param>
/// <param name="MetadataUrl">The URL that links to the metadata about the pool.</param>
public sealed record BakerPoolInfo(
    CommissionRates CommissionRates,
    BakerPoolOpenStatus OpenStatus,
    string MetadataUrl)
{
    internal static BakerPoolInfo From(Grpc.V2.BakerPoolInfo poolInfo) =>
        new
        (
            CommissionRates: CommissionRates.From(poolInfo.CommissionRates),
            OpenStatus: BakerPoolOpenStatus.OpenForAll,
            MetadataUrl: poolInfo.Url
        );
}
