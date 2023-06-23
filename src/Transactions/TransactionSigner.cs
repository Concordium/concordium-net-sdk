using System.Collections.Immutable;
using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Dictionary based implementation of <see cref="ITransactionSigner"/>.
///
/// Maintains a dictionary mapping pairs of <see cref="AccountCredentialIndex"/>
/// and <see cref="AccountKeyIndex"/> to <see cref="ISigner"/>s corresponding
/// to the sign keys of an account.
/// </summary>
public sealed class TransactionSigner : ITransactionSigner
{
    /// <summary>
    /// Internal representation of the signer map.
    /// </summary>
    private readonly Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> _signers;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionSigner"/> class.
    /// </summary>
    public TransactionSigner() =>
        this._signers = new Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>>();

    public byte GetSignatureCount()
        => (byte)this._signers.Values.SelectMany(x => x.Values).Count();

    public ImmutableDictionary<AccountCredentialIndex, ImmutableDictionary<AccountKeyIndex, ISigner>>
        GetSignerEntries() => this._signers
            .Select(
                x =>
                    new KeyValuePair<
                        AccountCredentialIndex,
                        ImmutableDictionary<AccountKeyIndex, ISigner>
                    >(x.Key, x.Value.ToImmutableDictionary())
            )
            .ToImmutableDictionary();

    /// <summary>
    /// Adds a sign key implementation to the <see cref="TransactionSigner"/>.
    /// </summary>
    /// <param name="credentialIndex">The credential index of the credential to which the key belongs.</param>
    /// <param name="keyIndex">The key index of the key, relative to the credential index.</param>
    /// <param name="signer">The sign key implementation.</param>
    public void AddSignerEntry(
        AccountCredentialIndex credentialIndex,
        AccountKeyIndex keyIndex,
        ISigner signer
    )
    {
        if (!this._signers.ContainsKey(credentialIndex))
        {
            this._signers.Add(credentialIndex, new Dictionary<AccountKeyIndex, ISigner>());
        }
        this._signers[credentialIndex].Add(keyIndex, signer);
    }

    /// <summary>
    /// Sign the provided transaction hash using all added sign key implementations.
    /// </summary>
    /// <param name="data">The transaction hash to sign.</param>
    /// <exception cref="ArgumentException">No signatures were produced.</exception>
    /// <exception cref="ArgumentException">A signature produced by signing is not 64 bytes.</exception>
    public AccountTransactionSignature Sign(byte[] data)
    {
        if (this.GetSignatureCount() == 0)
        {
            throw new ArgumentException("The signer will not produce any signatures.");
        }

        var signature =
            new Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, byte[]>>();
        foreach (var signerEntry in this._signers)
        {
            var accountSignatureMap = new Dictionary<AccountKeyIndex, byte[]>();
            foreach (var k in signerEntry.Value)
            {
                var signer = k.Value;
                accountSignatureMap.Add(k.Key, signer.Sign(data));
            }
            signature.Add(signerEntry.Key, accountSignatureMap);
        }

        return AccountTransactionSignature.Create(signature);
    }
}
