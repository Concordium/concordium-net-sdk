using Concordium.Sdk.Types;
using Concordium.Sdk.Crypto;
using System.Collections.Immutable;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Implementers of <see cref="ITransactionSigner"/> provide functionality for signing
/// data using account keys.
///
/// An account has one or more credentials with each such credential having up to
/// 256 sign keys (and accompanying verification keys). Each credential is identified
/// by its unique <see cref="AccountCredentialIndex"/> and each key by a unique pair
/// of a <see cref="AccountCredentialIndex"/> corresponding to the credential to which
/// it belongs, and a unique <see cref="AccountKeyIndex"/> relative to that credential
/// index of the account. For each key, it is thus represented by some pair of a credential
/// index and a key index.
///
/// The resulting <see cref="AccountTransactionSignature"/> from calling <see cref="Sign"/>
/// should reflect this structure and correspond to a mapping from credential and key indices
/// to the signatures produced by signing a transaction hash with the corresponding keys.
/// </summary>
public interface ITransactionSigner
{
    /// <summary>
    /// Gets the number of signatures that will be produced when signing a transaction using this signer.
    /// This number is based on the signer count.
    /// </summary>
    public byte GetSignatureCount();

    /// <summary>
    /// Sign a transaction hash.
    /// </summary>
    /// <param name="bytes">A byte array representing the transaction hash to sign.</param>
    /// <returns>A <see cref="AccountTransactionSignature"/> representing the transaction signature.</returns>
    public AccountTransactionSignature Sign(byte[] data);

    /// <summary>
    /// Gets a dictionary mapping pairs of credential and key indices to
    /// their corresponding sign key implementations.
    /// </summary>
    /// <returns>
    /// A dictionary mapping pairs of credential and key indices to the sign key implementation.
    /// </returns>
    public ImmutableDictionary<
        AccountCredentialIndex,
        ImmutableDictionary<AccountKeyIndex, ISigner>
    > GetSignerEntries();
}
