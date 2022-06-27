using ConcordiumNetSdk.SignKey;
using Index = ConcordiumNetSdk.Types.Index;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// A transaction signer is a map from the index of the credential to another map from the key index to the actual signer.
/// The credential index is relative to the account address, and the indices should be distinct.
/// The key index is relative to the credential.
/// The maximum length of the list is 255, and the minimum length is 1.
/// </summary>
public interface ITransactionSigner
{
    /// <summary>
    /// Gets the map from the index of the credential to another map from the key index to the actual signer.
    /// </summary>
    Dictionary<Index, Dictionary<Index, ISigner>> SignerEntries { get; }
    
    /// <summary>
    /// Gets the count of future signatures. Base on signer count.
    /// </summary>
    uint SignatureCount { get; }

    /// <summary>
    /// Adds the signer entry to signer entries collection. Signer entry contains of an index of the credential, the key index and the actual signer.
    /// </summary>
    /// <param name="credentialIndex">the index of the credential.</param>
    /// <param name="keyIndex">the key index.</param>
    /// <param name="signer">the actual signer.</param>
    void AddSignerEntry(Index credentialIndex, Index keyIndex, ISigner signer);
}
