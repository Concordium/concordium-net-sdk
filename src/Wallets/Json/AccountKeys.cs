using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object indexed by the
/// <c>accountKeys</c> field of the browser and genesis wallet
/// export JSON formats.
///
/// The object indexed by this field holds the sign keys and can
/// be parsed into a map from pairs of <see cref="AccountCredentialIndex"/>
/// and <see cref="AccountKeyIndex"/>es to <see cref="Ed25519SignKey"/>s
/// using <see cref="TryGetSignKeys"/>.
/// </summary>
public record AccountKeys
{
    public record KeyInfo
    {
        public record Key
        {
            public string SignKey { get; init; }
        };

        public Dictionary<string, Key> Keys { get; init; }
    }

    public Dictionary<string, KeyInfo> Keys { get; init; }

    /// <summary>
    /// Try to parse the sign keys into a map from pairs of
    /// <see cref="AccountCredentialIndex"/> and <see cref="AccountKeyIndex"/>es
    /// to <see cref="Ed25519SignKey"/>s representing the corresponding keys in
    /// the 'accountKeys' JSON object.
    /// </summary>
    /// <exception cref="ArgumentNullException">A field is missing.</exception>
    /// <exception cref="ArgumentNullException">An index or sign key could not be parsed.</exception>
    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetSignKeys()
    {
        if (this.Keys is null)
        {
            throw new WalletDataSourceException("Required field 'keys' is missing.");
        }

        return this.Keys
            .Select(cred =>
            {
                if (cred.Value.Keys is null)
                {
                    throw new WalletDataSourceException("Required field 'keys' is missing.");
                }

                // For each credential index, first parse it.
                var accountCredentialIndex = AccountCredentialIndex.From(cred.Key);

                // Then process its keys.
                var keysForCredential = cred.Value.Keys
                    .Select(key =>
                    {
                        if (key.Value.SignKey is null)
                        {
                            throw new WalletDataSourceException(
                                "Required field 'signKey' is missing."
                            );
                        }

                        // For each key index, parse it.
                        var accountKeyIndex = AccountKeyIndex.From(key.Key);

                        // Then parse the key.
                        ISigner signer = Ed25519SignKey.From(key.Value.SignKey);
                        return new KeyValuePair<AccountKeyIndex, ISigner>(accountKeyIndex, signer);
                    })
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                return new KeyValuePair<
                    AccountCredentialIndex,
                    Dictionary<AccountKeyIndex, ISigner>
                >(accountCredentialIndex, keysForCredential);
            })
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}
