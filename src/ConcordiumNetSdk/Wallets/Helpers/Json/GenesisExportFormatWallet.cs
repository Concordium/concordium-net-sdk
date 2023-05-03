using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Wallets.Helpers.Json;

public class GenesisExportFormatWallet : IImportedWallet
{
    public AccountKeys? accountKeys { get; set; }
    public string? address { get; set; }

    public AccountAddress TryGetAddress()
    {
        AccountAddress accountAddress = AccountAddress.From(address);
        return accountAddress;
    }

    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetKeys()
    {
        return accountKeys.keys
            .Select(cred =>
            {
                // For each credential index.
                AccountCredentialIndex accountCredentialIndex = AccountCredentialIndex.From(
                    cred.Key
                );
                Dictionary<AccountKeyIndex, ISigner> keysForCredential = cred.Value.keys
                    .Select(key =>
                    {
                        // For each key index.
                        AccountKeyIndex accountKeyIndex = AccountKeyIndex.From(key.Key);
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
