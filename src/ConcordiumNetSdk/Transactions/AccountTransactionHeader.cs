using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;
using ConcordiumNetSdk.Types;
using Concordium.V2;
using System.Buffers.Binary;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Header of an account transaction payload.
/// </summary>
public class AccountTransactionHeader
{
    /// <summary>
    /// The length of the serialized account transaction header.
    /// </summary>
    public const UInt32 BytesLength =
        AccountAddress.BytesLength
        + AccountNonce.BytesLength
        + sizeof(UInt64)
        + sizeof(UInt32)
        + Expiry.BytesLength;

    /// <summary>
    /// Address of the sender of the transaction.
    /// </summary>
    private readonly AccountAddress _sender;

    /// <summary>
    /// Account nonce to use for the transaction.
    /// </summary>
    private readonly AccountNonce _nonce;

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    private readonly Expiry _expiry;

    /// <summary>
    /// Maximum amount of energy to spend on this transaction.
    /// </summary>
    private readonly UInt64 _maxEnergyCost;

    /// <summary>
    /// Size of the transaction payload.
    /// </summary>
    private readonly UInt32 _payloadSize;

    /// <summary>
    /// The serialized header.
    /// </summary>
    private readonly byte[] _serializedHeader;

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
        AccountNonce nonce,
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
        _serializedHeader = Serialize(sender, nonce, expiry, maxEnergyCost, payloadSize);
    }

    /// <summary>
    /// Gets the account transaction serialized to the binary format expected by the node.
    /// This is used when signing transactions.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="maxEnergyCost">Maximum amount of energy to spend on this transaction.</param>
    /// <param name="payloadSize">Size of the transaction payload.</param>
    private static byte[] Serialize(
        AccountAddress sender,
        AccountNonce nonce,
        Expiry expiry,
        UInt64 maxEnergyCost,
        UInt32 payloadSize
    )
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(sender.GetBytes());
        memoryStream.Write(nonce.GetBytes());
        var maxEnergyCostBytes = new byte[sizeof(UInt64)];
        BinaryPrimitives.WriteUInt64BigEndian(new Span<byte>(maxEnergyCostBytes), maxEnergyCost);
        memoryStream.Write(maxEnergyCostBytes);
        var payloadSizeBytes = new byte[sizeof(UInt32)];
        BinaryPrimitives.WriteUInt32BigEndian(new Span<byte>(payloadSizeBytes), payloadSize);
        memoryStream.Write(payloadSizeBytes);
        memoryStream.Write(expiry.GetBytes());
        return memoryStream.ToArray();
    }

    public byte[] GetBytes()
    {
        return (byte[])_serializedHeader.Clone();
    }

    /// <summary>
    /// Converts the account transaction header to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.AccountTransactionHeader ToProto()
    {
        return new Concordium.V2.AccountTransactionHeader()
        {
            Sender = _sender.ToProto(),
            SequenceNumber = _nonce.ToProto(),
            Expiry = _expiry.ToProto(),
            EnergyAmount = new Concordium.V2.Energy() { Value = _maxEnergyCost }
        };
    }
}
