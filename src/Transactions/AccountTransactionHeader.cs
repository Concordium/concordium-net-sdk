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
public struct AccountTransactionHeader
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
    /// Address of the sender of the transaction.
    /// </summary>
    public readonly AccountAddress Sender { get; init; }

    /// <summary>
    /// Account sequence number to use for the transaction.
    /// </summary>
    public AccountSequenceNumber SequenceNumber { get; init; }

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    public Expiry Expiry { get; init; }

    /// <summary>
    /// Maximum amount of energy to spend on this transaction.
    /// </summary>
    public EnergyAmount MaxEnergyCost { get; init; }

    /// <summary>
    /// Size of the transaction payload.
    /// </summary>
    public PayloadSize PayloadSize { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountTransactionHeader"/> class.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="maxEnergyCost">Maximum amount of energy to spend on this transaction.</param>
    /// <param name="payloadSize">Size of the transaction payload.</param>
    public AccountTransactionHeader(
        AccountAddress sender,
        AccountSequenceNumber nonce,
        Expiry expiry,
        EnergyAmount maxEnergyCost,
        PayloadSize payloadSize
    )
    {
        this.Sender = sender;
        this.SequenceNumber = nonce;
        this.Expiry = expiry;
        this.MaxEnergyCost = maxEnergyCost;
        this.PayloadSize = payloadSize;
    }

    /// <summary>
    /// Get the account transaction serialized to the binary format expected by the node.
    /// This is used when signing transactions.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="maxEnergyCost">Maximum amount of energy to spend on this transaction.</param>
    /// <param name="payloadSize">Size of the transaction payload.</param>
    private static byte[] Serialize(
        AccountAddress sender,
        AccountSequenceNumber nonce,
        Expiry expiry,
        EnergyAmount maxEnergyCost,
        PayloadSize payloadSize
    )
    {
        using var memoryStream = new MemoryStream();
        memoryStream.Write(sender.GetBytes());
        memoryStream.Write(nonce.GetBytes());
        memoryStream.Write(maxEnergyCost.GetBytes());
        memoryStream.Write(payloadSize.GetBytes());
        memoryStream.Write(expiry.GetBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Get the transaction header in the binary format expected by the node.
    /// </summary>
    public readonly byte[] GetBytes() =>
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
    public readonly Grpc.V2.AccountTransactionHeader ToProto() =>
        new()
        {
            Sender = this.Sender.ToProto(),
            SequenceNumber = this.SequenceNumber.ToProto(),
            Expiry = this.Expiry.ToProto(),
            EnergyAmount = new Grpc.V2.Energy() { Value = MaxEnergyCost }
        };
}
