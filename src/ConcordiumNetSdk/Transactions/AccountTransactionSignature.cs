using System.Collections.Immutable;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Signature of an account transaction.
/// An account transaction signature is a map from indices of credentials to maps from key indices to signatures.
/// A credential index is relative to the account address and a key index is relative to the credential.
/// The number of credentials and keys per credential should be at least 1 and at most 255.
/// </summary>
public class AccountTransactionSignature
{
    /// <summary>
    /// Internal representation of the signature. This is a map from account credential indices to account signature maps.
    /// </summary>
    public readonly ImmutableDictionary<byte, AccountSignatureMap> signature;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountTransactionSignature"/> class.
    /// </summary>
    /// <param name="signatureMaps"><see cref="Dictionary"> corresponding to the map.</param>
    private AccountTransactionSignature(Dictionary<byte, Dictionary<byte, byte[]>> signatureMaps)
    {
        var accountTransactionSignature = new Dictionary<byte, AccountSignatureMap>();
        foreach (var m in signatureMaps)
        {
            var accountSignatureMap = new Dictionary<byte, byte[]>();
            foreach (var k in m.Value)
            {
                accountSignatureMap.Add(k.Key, k.Value);
            }
            accountTransactionSignature.Add(m.Key, new AccountSignatureMap(accountSignatureMap));
        }
        this.signature = accountTransactionSignature.ToImmutableDictionary();
    }

    /// <summary>
    /// Creates an account transaction signature by signing the provided data with the specified transaction signer.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signatureMaps">The <see cref="ITransactionSigner"> instance to use for signing the data.</param>
    public static AccountTransactionSignature Create(
        ITransactionSigner transactionSigner,
        byte[] data
    )
    {
        if (transactionSigner.GetSignatureCount() == 0)
        {
            throw new ArgumentException("The provided signer will not produce any signatures.");
        }

        var signature = new Dictionary<byte, Dictionary<byte, byte[]>>();
        foreach (var signerEntry in transactionSigner.GetSignerEntries())
        {
            var accountSignatureMap = new Dictionary<byte, byte[]>();
            foreach (var k in signerEntry.Value)
            {
                var index = k.Key;
                var signer = k.Value;
                accountSignatureMap.Add(k.Key, signer.Sign(data));
            }
            signature.Add(signerEntry.Key, accountSignatureMap);
        }

        return new AccountTransactionSignature(signature);
    }

    /// <summary>
    /// Converts the account transaction signature to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.AccountTransactionSignature ToProto()
    {
        var accountTransactionSignature = new Concordium.V2.AccountTransactionSignature();
        signature
            .ToList()
            .ForEach(x => accountTransactionSignature.Signatures.Add(x.Key, x.Value.ToProto()));
        return accountTransactionSignature;
    }
}
