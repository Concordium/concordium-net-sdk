using System.Collections.Immutable;
using ConcordiumNetSdk.SignKey;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// A transaction signer is a map from indices of credentials to maps from key indices to signers.
/// A credential index is relative to the account address and a key index is relative to the credential.
/// The number of credentials and keys per credential should be at least 1 and at most 255.
/// </summary>
public interface ITransactionSigner
{
    /// <summary>
    /// Get the signer entries represented as a map from credential indices to another map from key indices to signers.
    /// For a given credential and key index, its corresponding signer can be used to sign data whose resulting signature
    /// can then be verified using its corresponding verification key.
    /// </summary>
    ImmutableDictionary<byte, ImmutableDictionary<byte, ISigner>> GetSignerEntries();

    /// <summary>
    /// Get the number of signatures that will be produced when signing a transaction using this signer.
    /// This number is based on the signer count.
    /// </summary>
    byte GetSignatureCount();

    /// <summary>
    /// Adds the specified signer entry to the signer. A signer entry consists of an index of the credential, a key index and a signer.
    /// </summary>
    /// <param name="credentialIndex">The credential index.</param>
    /// <param name="keyIndex">The key index to the signer for the corresponding key and index.</param>
    /// <param name="signer">The signer.</param>
    void AddSignerEntry(byte credentialIndex, byte keyIndex, ISigner signer);
}
