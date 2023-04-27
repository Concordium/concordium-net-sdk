using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.SignKey;
using System.Collections.Immutable;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Models a map from <see cref="AccountKeyIndex"/> to signatures produced by signing the
/// account transaction hashes with the key corresponding to an index. This key index is
/// always relative to a <see cref="AccountCredentialIndex"/>.
/// </summary>
public class AccountSignatureMap
{
    /// <summary>
    /// Internal representation of the map, from key indices to signatures.
    /// </summary>
    private ImmutableDictionary<AccountKeyIndex, byte[]> _signatures;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountSignatureMap"/> class.
    /// </summary>
    /// <param name="signatures">A map from key indices to signatures.</param>
    /// <exception cref="ArgumentException">If some signature is not 64 bytes.</exception>
    public AccountSignatureMap(Dictionary<AccountKeyIndex, byte[]> signatures)
    {
        // Signature lengths are fixed. This is a bit hacky, but check it here.
        if (
            signatures.Values.Any(
                signature => signature.Length != Ed25519SignKey.SignatureBytesLength
            )
        )
        {
            throw new ArgumentException(
                $"Signature should be {Ed25519SignKey.SignatureBytesLength} bytes."
            );
        }
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
