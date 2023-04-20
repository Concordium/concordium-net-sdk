using ConcordiumNetSdk.Types;
using System.Collections.Immutable;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Map from key indices to signatures resulting from signing the account transaction with the corresponding key.
/// </summary>
public class AccountSignatureMap
{
    /// <summary>
    /// Internal representation of the map, from key indices to signatures.
    /// </summary>
    private ImmutableDictionary<byte, byte[]> _signatures;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountSignatureMap"/> class.
    /// </summary>
    /// <param name="signatures">A map from key indices to signatures.</param>
    public AccountSignatureMap(Dictionary<byte, byte[]> signatures)
    {
        this._signatures = signatures.ToImmutableDictionary();
    }

    /// <summary>
    /// Converts the account signature map to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.AccountSignatureMap ToProto()
    {
        var accountSignatureMap = new Concordium.V2.AccountSignatureMap();
        foreach (var s in this._signatures)
        {
            accountSignatureMap.Signatures.Add(
                s.Key,
                new Concordium.V2.Signature()
                {
                    Value = Google.Protobuf.ByteString.CopyFrom(s.Value)
                }
            );
        }
        return accountSignatureMap;
    }
}
