using System.Collections.Immutable;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents the signature of a signed account transaction.
///
/// An account has one or more credentials with each such credential having up to
/// 256 sign keys (and accompanying verification keys). Each credential is identified
/// by its unique <see cref="AccountCredentialIndex"/> and each key by a unique pair
/// of a <see cref="AccountCredentialIndex"/> corresponding to the credential to which
/// it belongs, and a unique <see cref="AccountKeyIndex"/> relative to that credential
/// index of the account. For each key, it is thus represented by some pair of a credential
/// index and a key index.
///
/// An <see cref="AccountTransactionSignature"/> corresponds to a mapping from credential
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
    public ImmutableDictionary<
        AccountCredentialIndex,
        AccountSignatureMap
    > SignatureMap
    { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountTransactionSignature"/> class.
    /// </summary>
    /// <param name="signatureMaps">Dictionary corresponding to the map.</param>
    private AccountTransactionSignature(
        Dictionary<AccountCredentialIndex, AccountSignatureMap> signatureMaps
    ) => this.SignatureMap = signatureMaps.ToImmutableDictionary();

    /// <summary>
    /// Creates a new instance of the <see cref="AccountTransactionSignature"/> class.
    /// </summary>
    /// <param name="signatureMaps">Dictionary corresponding to the map.</param>
    /// <exception cref="ArgumentException">A signature is not 64 bytes.</exception>
    public static AccountTransactionSignature Create(
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
            accountTransactionSignature.Add(m.Key, AccountSignatureMap.Create(accountSignatureMap));
        }
        return new AccountTransactionSignature(accountTransactionSignature);
    }

    /// <summary>
    /// Converts the account transaction signature to its corresponding protocol buffer message instance.
    /// </summary>
    public Grpc.V2.AccountTransactionSignature ToProto()
    {
        var accountTransactionSignature = new Grpc.V2.AccountTransactionSignature();
        this.SignatureMap
            .ToList()
            .ForEach(x => accountTransactionSignature.Signatures.Add(x.Key, x.Value.ToProto()));
        return accountTransactionSignature;
    }
}
