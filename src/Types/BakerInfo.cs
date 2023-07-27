namespace Concordium.Sdk.Types;

/// <summary>
/// Information about a baker.
/// </summary>
/// <param name="BakerId">Account index of the account controlling the baker.</param>
/// <param name="BakerElectionVerifyKey">
/// Baker's public key used to check whether they won the lottery or not.
/// </param>
/// <param name="BakerSignatureVerifyKey"></param>
/// <param name="BakerAggregationVerifyKey">
/// Baker's public key used to check signatures on finalization records.
/// This is only used if the baker has sufficient stake to participate in
/// finalization.
/// </param>
public sealed record BakerInfo(BakerId BakerId,
    byte[] BakerElectionVerifyKey,
    byte[] BakerSignatureVerifyKey,
    byte[] BakerAggregationVerifyKey)
{
    internal static BakerInfo From(Grpc.V2.BakerInfo bakerInfo) =>
        new(
            BakerId.From(bakerInfo.BakerId),
            bakerInfo.ElectionKey.Value.ToByteArray(),
            bakerInfo.SignatureKey.Value.ToByteArray(),
            bakerInfo.AggregationKey.Value.ToByteArray()
        );
}
