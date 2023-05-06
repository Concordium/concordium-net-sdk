using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Crypto;
using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object indexed by the
/// <c>accountKeys</c> field of the browser and genesis wallet
/// export JSON formats.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
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
            [JsonProperty(Required = Required.DisallowNull)]
            internal string signKey { get; init; }
        };

        [JsonProperty(Required = Required.DisallowNull)]
        internal Dictionary<string, Key> keys { get; init; }
    }

    [JsonProperty(Required = Required.DisallowNull)]
    internal Dictionary<string, KeyInfo> keys { get; init; }

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
        return keys.Select(cred =>
            {
                // For each credential index, first parse it.
                AccountCredentialIndex accountCredentialIndex = AccountCredentialIndex.From(
                    cred.Key
                );

                // Then process its keys.
                Dictionary<AccountKeyIndex, ISigner> keysForCredential = cred.Value.keys
                    .Select(key =>
                    {
                        // For each key index, parse it.
                        AccountKeyIndex accountKeyIndex = AccountKeyIndex.From(key.Key);

                        // Then parse the key.
                        ISigner signer = Ed25519SignKey.From(key.Value.signKey);
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
