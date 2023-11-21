namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents the raw payload of an account transaction.
///
/// Used mostly for debugging, the only place where this will be encountered
/// is when querying transactions from chain that have not been implemented for
/// this SDK yet, and thus can not be deserialized.
/// </summary>
/// <param name="bytes">The raw bytes of the payload.</param>
public sealed record RawPayload(byte[] bytes) : AccountTransactionPayload
{
    public override byte[] ToBytes() => this.bytes;
}
