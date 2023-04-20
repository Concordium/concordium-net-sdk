using System.Collections.Immutable;
using ConcordiumNetSdk.SignKey;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Dictionary based implementation of <see cref="ITransactionSigner"/>.
/// </summary>
public class TransactionSigner : ITransactionSigner
{
    /// <summary>
    /// Internal representation of the mapping of <see cref="ITransactionSigner"/>.
    /// </summary>
    private readonly Dictionary<byte, Dictionary<byte, ISigner>> _signers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionSigner"/> class.
    /// </summary>
    public TransactionSigner()
    {
        _signers = new Dictionary<byte, Dictionary<byte, ISigner>>();
    }

    public byte GetSignatureCount()
    {
        return (byte)_signers.Values.SelectMany(x => x.Values).Count();
    }

    public ImmutableDictionary<byte, ImmutableDictionary<byte, ISigner>> GetSignerEntries()
    {
        return _signers
            .Select(
                x =>
                    new KeyValuePair<byte, ImmutableDictionary<byte, ISigner>>(
                        x.Key,
                        x.Value.ToImmutableDictionary()
                    )
            )
            .ToImmutableDictionary();
    }

    public void AddSignerEntry(byte credentialIndex, byte keyIndex, ISigner signer)
    {
        if (!_signers.ContainsKey(credentialIndex))
        {
            _signers.Add(credentialIndex, new Dictionary<byte, ISigner>());
        }
        _signers[credentialIndex].Add(keyIndex, signer);
    }
}
