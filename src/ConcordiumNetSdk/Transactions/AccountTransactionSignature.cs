using ConcordiumNetSdk.Types;
using System.Collections.Immutable;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents the signature of a signed account transaction.
///
/// An account has one or more credentials with each such credential having up to
/// 255 sign keys (and accompanying verification keys). Each credential is identified
/// by its unique <see cref="AccountCredentialIndex"/> and each key by a unique pair
/// of a <see cref="AccountCredentialIndex"/> corresponding to the credential to which
/// it belongs, and a unique <see cref="AccountKeyIndex"/> relative to that credential
/// index of the account. For each key, it is thus represented by some pair of a credential
/// index and a key index.
///
/// An <see cref="AccountTransactionSignature"> corresponds to a mapping from credential
/// and key indices to signatures produced by signing the transaction hash with the account
/// keys corresponding to the indices.
///
/// Producers of <see cref="AccountTransactionSignature"/>s should implement
/// <see cref="ITransactionSigner"/>.
/// </summary>
public class AccountTransactionSignature
{
    /// <summary>
    /// Internal representation of the signature. This is a map from account credential indices to account signature maps.
    /// </summary>
    public readonly ImmutableDictionary<AccountCredentialIndex, AccountSignatureMap> signature;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountTransactionSignature"/> class.
    /// </summary>
    /// <param name="signatureMaps"><see cref="Dictionary"> corresponding to the map.</param>
    public AccountTransactionSignature(
        Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, byte[]>> signatureMaps
    )
    {
        // Outer dictionary for the credential indices.
        var accountTransactionSignature =
            new Dictionary<AccountCredentialIndex, AccountSignatureMap>();
        foreach (var m in signatureMaps)
        {
            // Outer inner dictionary for the key indices.
            var accountSignatureMap = new Dictionary<AccountKeyIndex, byte[]>();
            foreach (var k in m.Value)
            {
                accountSignatureMap.Add(k.Key, k.Value);
            }
            accountTransactionSignature.Add(m.Key, new AccountSignatureMap(accountSignatureMap));
        }
        this.signature = accountTransactionSignature.ToImmutableDictionary();
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
