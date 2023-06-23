using System.Collections.Immutable;
using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a map from <see cref="AccountKeyIndex"/> values to signatures.
///
/// An <see cref="AccountSignatureMap"/> corresponds to a mapping from key indices
/// to signatures produced by signing a transaction hash with the account keys
/// corresponding to the indices. A key index is always relative to an
/// <see cref="AccountCredentialIndex"/>. Each signature is currently produced by
/// signing with an <see cref="Ed25519SignKey"/> and therefore 64 bytes long.
/// </summary>
public sealed record AccountSignatureMap
{
    /// <summary>
    /// Internal representation of the map.
    /// </summary>
    public ImmutableDictionary<AccountKeyIndex, byte[]> Signatures { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountSignatureMap"/> class.
    /// </summary>
    /// <param name="signatures">A map from account key indices to signatures.</param>
    private AccountSignatureMap(Dictionary<AccountKeyIndex, byte[]> signatures) => this.Signatures = signatures.ToImmutableDictionary();

    /// <summary>
    /// Creates a new instance of the <see cref="AccountSignatureMap"/> class.
    /// </summary>
    /// <param name="signatures">A map from account key indices to signatures.</param>
    /// <exception cref="ArgumentException">A signature is not 64 bytes.</exception>
    public static AccountSignatureMap Create(Dictionary<AccountKeyIndex, byte[]> signatures)
    {
        // Signatures are 64-byte ed25519 signatures and therefore 64 bytes.
        if (signatures.Values.Any(signature => signature.Length != 64))
        {
            throw new ArgumentException($"Signature should be {64} bytes.");
        }
        return new AccountSignatureMap(signatures);
    }

    /// <summary>
    /// Converts the account signature map to its corresponding protocol buffer message instance.
    /// </summary>
    public Grpc.V2.AccountSignatureMap ToProto()
    {
        var accountSignatureMap = new Grpc.V2.AccountSignatureMap();
        foreach (var s in this.Signatures)
        {
            accountSignatureMap.Signatures.Add(
                s.Key.Value,
                new Grpc.V2.Signature() { Value = Google.Protobuf.ByteString.CopyFrom(s.Value) }
            );
        }
        return accountSignatureMap;
    }
}
