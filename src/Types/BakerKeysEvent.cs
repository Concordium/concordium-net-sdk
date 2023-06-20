namespace Concordium.Sdk.Types;

/// <summary>
/// Result of a successful change of baker keys.
/// </summary>
/// <param name="BakerId">ID of the baker whose keys were changed.</param>
/// <param name="Account">Account address of the baker.</param>
/// <param name="SignKey">The new public key for verifying block signatures..</param>
/// <param name="ElectionKey">
/// The new public key for verifying whether the baker won the block
/// lottery.
/// </param>
/// <param name="AggregationKey">The new public key for verifying finalization records.</param>
public sealed record BakerKeysEvent(
    BakerId BakerId,
    AccountAddress Account,
    byte[] SignKey,
    byte[] ElectionKey,
    byte[] AggregationKey
)
{
    internal static BakerKeysEvent From(Grpc.V2.BakerKeysEvent bakerKeysEvent) =>
        new(
            BakerId.From(bakerKeysEvent.BakerId),
            AccountAddress.From(bakerKeysEvent.Account),
            bakerKeysEvent.SignKey.Value.ToByteArray(),
            bakerKeysEvent.ElectionKey.Value.ToByteArray(),
            bakerKeysEvent.AggregationKey.Value.ToByteArray()
        );
}
