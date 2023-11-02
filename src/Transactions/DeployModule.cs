
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>A deployment of a Wasm smart contract module.</summary>
/// <param name="Module">The smart contract module to be deployed.</param>
public sealed record DeployModule(VersionedModuleSource Module) : AccountTransactionPayload
{
    /// <summary>The account transaction type to be used in the serialized payload.</summary>
    private const byte TransactionType = (byte)Types.TransactionType.DeployModule;

    /// <summary>
    /// Copies the "transfer with memo" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="Module">The smart contract module to be deployed.</param>
    private static byte[] Serialize(VersionedModuleSource module)
    {
        using var memoryStream = new MemoryStream((int)(
            sizeof(TransactionType) +
            AccountAddress.BytesLength +
            module.Source.Length +
            CcdAmount.BytesLength));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        memoryStream.Write(memo.ToBytes());
        memoryStream.Write(amount.ToBytes());
        return memoryStream.ToArray();
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver, this.Memo);
}
