using Concordium.Sdk.Types;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents the header of an account transaction.
///
/// Transactions sent to the node include an account transaction header with
/// the following information that is used when processing the transaction:
///
/// The <see cref="AccountAddress"/> of the sender,
/// the <see cref="AccountSequenceNumber"/> to use,
/// the <see cref="Expiry"/> time of the transaction,
/// the maximum <see cref="EnergyAmount"/> to spend on the transaction as well as
/// the <see cref="PayloadSize"/>.
/// </summary>
/// <param name="Sender">Address of the sender of the transaction.</param>
/// <param name="SequenceNumber">Account sequence number to use for the transaction.</param>
/// <param name="Expiry">Expiration time of the transaction.</param>
/// <param name="MaxEnergyCost">Maximum amount of energy to spend on this transaction.</param>
/// <param name="PayloadSize">Size of the transaction payload.</param>
public sealed record AccountTransactionHeader(
    AccountAddress Sender,
    AccountSequenceNumber SequenceNumber,
    Expiry Expiry,
    EnergyAmount MaxEnergyCost,
    PayloadSize PayloadSize
    )
{
    /// <summary>
    /// The length of the serialized account transaction header in bytes.
    /// </summary>
    public const uint BytesLength =
        AccountAddress.BytesLength
        + AccountSequenceNumber.BytesLength
        + EnergyAmount.BytesLength
        + PayloadSize.BytesLength
        + Expiry.BytesLength;

    /// <summary>
    /// Copies the account transaction serialized to the binary format expected by the node
    /// to a byte array. This is used for signing transactions.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="sequenceNumber">Account sequence number to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="maxEnergyCost">Maximum amount of energy to spend on this transaction.</param>
    /// <param name="payloadSize">Size of the transaction payload.</param>
    private static byte[] Serialize(
        AccountAddress sender,
        AccountSequenceNumber sequenceNumber,
        Expiry expiry,
        EnergyAmount maxEnergyCost,
        PayloadSize payloadSize
    )
    {
        using var memoryStream = new MemoryStream((int)BytesLength);
        memoryStream.Write(sender.ToBytes());
        memoryStream.Write(sequenceNumber.ToBytes());
        memoryStream.Write(maxEnergyCost.ToBytes());
        memoryStream.Write(payloadSize.ToBytes());
        memoryStream.Write(expiry.ToBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Copies the transaction header in the binary format expected by the node to a byte array.
    /// </summary>
    public byte[] ToBytes() =>
        Serialize(
            this.Sender,
            this.SequenceNumber,
            this.Expiry,
            this.MaxEnergyCost,
            this.PayloadSize
        );

    /// <summary>
    /// Converts the account transaction header to its corresponding protocol buffer message instance.
    /// </summary>
    public Grpc.V2.AccountTransactionHeader ToProto() =>
        new()
        {
            Sender = this.Sender.ToProto(),
            SequenceNumber = this.SequenceNumber.ToProto(),
            Expiry = this.Expiry.ToProto(),
            EnergyAmount = new Grpc.V2.Energy() { Value = this.MaxEnergyCost.Value }
        };

    /// <summary>
    /// Converts the account transaction header to its corresponding protocol buffer message instance.
    /// </summary>
    internal static AccountTransactionHeader From(Grpc.V2.AccountTransactionHeader accountTransactionHeader) {
        return new AccountTransactionHeader(
            AccountAddress.From(accountTransactionHeader.Sender), 
            AccountSequenceNumber.From(accountTransactionHeader.SequenceNumber),
            Expiry.From(accountTransactionHeader.Expiry.Value),
            EnergyAmount.From(accountTransactionHeader.EnergyAmount),
            new PayloadSize((uint) accountTransactionHeader.CalculateSize())
        );
    }
}
