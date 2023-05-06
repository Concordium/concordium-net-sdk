using AccountAddress = Concordium.Sdk.Types.AccountAddress;
using Concordium.Sdk.Types;

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
    public const UInt32 BytesLength =
        AccountAddress.BytesLength
        + AccountSequenceNumber.BytesLength
        + EnergyAmount.BytesLength
        + PayloadSize.BytesLength
        + Expiry.BytesLength;

    /// <summary>
    /// Address of the sender of the transaction.
    /// </summary>
    public readonly AccountAddress _sender;

    /// <summary>
    /// Account sequence number to use for the transaction.
    /// </summary>
    public readonly AccountSequenceNumber _nonce;

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    public readonly Expiry _expiry;

    /// <summary>
    /// Maximum amount of energy to spend on this transaction.
    /// </summary>
    public readonly EnergyAmount _maxEnergyCost;

    /// <summary>
    /// Size of the transaction payload.
    /// </summary>
    public readonly PayloadSize _payloadSize;

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
        UInt64 maxEnergyCost,
        UInt32 payloadSize
    )
    {
        _sender = sender;
        _nonce = nonce;
        _expiry = expiry;
        _maxEnergyCost = maxEnergyCost;
        _payloadSize = payloadSize;
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
        using MemoryStream memoryStream = new MemoryStream();
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
    public byte[] GetBytes()
    {
        return Serialize(_sender, _nonce, _expiry, _maxEnergyCost, _payloadSize);
    }

    /// <summary>
    /// Converts the account transaction header to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.Grpc.V2.AccountTransactionHeader ToProto()
    {
        return new Concordium.Grpc.V2.AccountTransactionHeader()
        {
            Sender = _sender.ToProto(),
            SequenceNumber = _nonce.ToProto(),
            Expiry = _expiry.ToProto(),
            EnergyAmount = new Concordium.Grpc.V2.Energy() { Value = _maxEnergyCost }
        };
    }
}