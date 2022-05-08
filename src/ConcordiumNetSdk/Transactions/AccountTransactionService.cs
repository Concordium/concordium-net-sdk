using System.Buffers.Binary;
using System.Security.Cryptography;
using ConcordiumNetSdk.Responses.NextAccountNonceResponse;
using ConcordiumNetSdk.Types;
using Index = ConcordiumNetSdk.Types.Index;

namespace ConcordiumNetSdk.Transactions;

// todo: implement tests
public class AccountTransactionService : IAccountTransactionService
{
    private const byte Version = 0;
    private static readonly TimeSpan DefaultTxExpiry = TimeSpan.FromMinutes(30);

    private readonly IConcordiumNodeClient _client;

    public AccountTransactionService(IConcordiumNodeClient client)
    {
        _client = client;
    }

    public async Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        IAccountTransactionPayload transactionPayload,
        ITransactionSigner signer)
    {
        NextAccountNonce? nextAccountNonce = await _client.GetNextAccountNonceAsync(sender);
        if (nextAccountNonce is null) throw new InvalidOperationException("Gained next account nonce was null. Can be invalid account address.");
        DateTimeOffset expiry = DateTimeOffset.UtcNow.Add(DefaultTxExpiry);
        return await SendAccountTransactionAsync(sender, nextAccountNonce.Nonce, transactionPayload, expiry, signer);
    }

    public async Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        Nonce nextAccountNonce,
        IAccountTransactionPayload transactionPayload,
        ITransactionSigner transactionSigner)
    {
        DateTimeOffset expiry = DateTimeOffset.UtcNow.Add(DefaultTxExpiry);
        return await SendAccountTransactionAsync(sender, nextAccountNonce, transactionPayload, expiry, transactionSigner);
    }

    public async Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        Nonce nextAccountNonce,
        IAccountTransactionPayload transactionPayload,
        DateTimeOffset expiry,
        ITransactionSigner transactionSigner)
    {
        byte[] serializedTxPayload = transactionPayload.SerializeToBytes();
        int signatureCount = transactionSigner.SignatureCount;
        int txPayloadSize = serializedTxPayload.Length;
        int txSpecificCost = transactionPayload.GetBaseEnergyCost();
        int energyCost = CalculateEnergyCost(signatureCount, txPayloadSize, txSpecificCost);
        byte[] serializedHeader = SerializeHeader(sender, txPayloadSize, nextAccountNonce, energyCost, expiry);
        byte[] serializedHeaderAndTxPayload = serializedHeader.Concat(serializedTxPayload).ToArray();
        byte[] signDigest = SHA256.Create().ComputeHash(serializedHeaderAndTxPayload);
        byte[] serializedSignatures = SerializeSignatures(transactionSigner, signDigest);
        byte[] serializedBlockItemKind = SerializeBlockItemKind(BlockItemKind.AccountTransactionKind);
        byte[] serializedAccountTx = serializedBlockItemKind.Concat(serializedSignatures).Concat(serializedHeaderAndTxPayload).ToArray();
        byte[] serializedVersion = SerializeVersion(Version);
        byte[] serializedPayload = serializedVersion.Concat(serializedAccountTx).ToArray();

        var isSuccessful = await _client.SendTransactionAsync(serializedPayload);
        if (!isSuccessful) throw new InvalidOperationException("Response indicated that transaction was not successfully sent.");
        
        byte[] txHash = SHA256.Create().ComputeHash(serializedAccountTx);
        return new TransactionHash(txHash);
    }

    private int CalculateEnergyCost(
        int signatureCount,
        int payloadSize,
        int transactionSpecificCost)
    {
        const int constantA = 100;
        const int constantB = 1;

        // Account address (32 bytes), nonce (8 bytes), energy (8 bytes), payload size (4 bytes), expiry (8 bytes);
        const int accountTransactionHeaderSize = 60;

        int result = transactionSpecificCost +
                     constantA * signatureCount +
                     constantB * (accountTransactionHeaderSize + payloadSize);

        return result;
    }

    private byte[] SerializeHeader(
        AccountAddress sender,
        int payloadSize,
        Nonce accountNonce,
        int energyCost,
        DateTimeOffset expiry)
    {
        Span<byte> serializedHeader = new byte[60];
        sender.AsBytes.CopyTo(serializedHeader.Slice(0, 32));
        BinaryPrimitives.WriteUInt64BigEndian(serializedHeader.Slice(32, 8), accountNonce.AsUInt64);
        BinaryPrimitives.WriteUInt64BigEndian(serializedHeader.Slice(40, 8), (ulong) energyCost);
        BinaryPrimitives.WriteUInt32BigEndian(serializedHeader.Slice(48, 4), (uint) payloadSize);
        BinaryPrimitives.WriteUInt64BigEndian(serializedHeader.Slice(52, 8), (ulong) expiry.ToUnixTimeSeconds());

        return serializedHeader.ToArray();
    }

    private byte[] SerializeSignatures(ITransactionSigner transactionSigner, byte[] signDigest)
    {
        byte[] serializedSignatures = new byte[1];
        byte credentialIndexCount = (byte) transactionSigner.SignerEntries.Count;
        serializedSignatures[0] = credentialIndexCount;

        foreach (var signerEntry in transactionSigner.SignerEntries)
        {
            byte[] serializedSignerEntry = new byte[2];
            Index credentialIndex = signerEntry.Key;
            int keyIndexCount = signerEntry.Value.Count;
            serializedSignerEntry[0] = credentialIndex.Value;
            serializedSignerEntry[1] = (byte) keyIndexCount;
            serializedSignatures = serializedSignatures.Concat(serializedSignerEntry).ToArray();

            foreach (var signer in signerEntry.Value)
            {
                byte[] signature = signer.Value.Sign(signDigest);
                ushort signatureLength = (ushort) signature.Length;

                Span<byte> serializedSigner = new byte[3 + signatureLength];
                Index keyIndex = signer.Key;
                serializedSigner[0] = keyIndex.Value;
                BinaryPrimitives.WriteUInt16BigEndian(serializedSigner.Slice(1, 2), signatureLength);
                signature.CopyTo(serializedSigner[3..]);
                serializedSignatures = serializedSignatures.Concat(serializedSigner.ToArray()).ToArray();
            }
        }

        return serializedSignatures;
    }

    private byte[] SerializeBlockItemKind(BlockItemKind blockItemKind)
    {
        return new byte[] {(byte) blockItemKind};
    }

    private byte[] SerializeVersion(byte version)
    {
        return new byte[] {version};
    }
}
