using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;
using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object indexed by the
/// <c>accountKeys</c> field of the browser and genesis wallet
/// export JSON formats.
///
/// Such can be parsed into an instance of this class using
/// <see cref="JsonConvert"/>.
///
/// The object indexed by this field holds the sign keys and can
/// be parsed into a map from pairs of <see cref="AccountCredentialIndex"/>
/// and <see cref="AccountKeyIndex"/>es to <see cref="Ed25519SignKey"/>s
/// using <see cref="TryGetSignKeys"/>.
/// </summary>
internal record AccountKeys
{
    internal record KeyInfo
    {
        internal record Key
        {
            [JsonProperty("signKey", Required = Required.DisallowNull)]
            internal string? SignKeyField { get; init; }
        };

        [JsonProperty("keys", Required = Required.DisallowNull)]
        internal Dictionary<string, Key>? KeysField { get; init; }
    }

    [JsonProperty("keys", Required = Required.DisallowNull)]
    internal Dictionary<string, KeyInfo>? KeysField { get; init; }

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
        if (this.KeysField is null)
        {
            throw new WalletDataSourceException("Required field 'keys' is missing.");
        }

        return this.KeysField
            .Select(cred =>
            {
                if (cred.Value.KeysField is null)
                {
                    throw new WalletDataSourceException("Required field 'keys' is missing.");
                }

                // For each credential index, first parse it.
                var accountCredentialIndex = AccountCredentialIndex.From(cred.Key);

                // Then process its keys.
                var keysForCredential = cred.Value.KeysField
                    .Select(key =>
                    {
                        if (key.Value.SignKeyField is null)
                        {
                            throw new WalletDataSourceException(
                                "Required field 'signKey' is missing."
                            );
                        }

                        // For each key index, parse it.
                        var accountKeyIndex = AccountKeyIndex.From(key.Key);

                        // Then parse the key.
                        ISigner signer = Ed25519SignKey.From(key.Value.SignKeyField);
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
