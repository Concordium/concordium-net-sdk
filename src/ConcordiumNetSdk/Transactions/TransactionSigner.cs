using ConcordiumNetSdk.SignKey;
using Index = ConcordiumNetSdk.Types.Index;

namespace ConcordiumNetSdk.Transactions;

// todo: implement tests
public class TransactionSigner : ITransactionSigner
{
    private const int MaxCredentialIndexes = 255;
    private const int MaxKeyIndexes = 255;
    
    private readonly Dictionary<Index, Dictionary<Index, ISigner>> _signers = new();

    public Dictionary<Index, Dictionary<Index, ISigner>> SignerEntries => _signers;

    public uint SignatureCount => (uint) _signers.Values.SelectMany(x => x.Values).Count();

    public void AddSignerEntry(Index credentialIndex, Index keyIndex, ISigner signer)
    {
        if (_signers.Count > MaxCredentialIndexes) throw new InvalidOperationException($"The maximum length of the credential indexes is {MaxCredentialIndexes}.");

        if (!_signers.ContainsKey(credentialIndex)) 
        {
            _signers.Add(credentialIndex, new Dictionary<Index, ISigner>());
        }

        if (_signers[credentialIndex].Count > MaxKeyIndexes) throw new InvalidOperationException($"The maximum length of the key indexes is {MaxKeyIndexes}.");

        _signers[credentialIndex].Add(keyIndex, signer);
    }
}
