using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// genesis wallet export JSON format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
public class GenesisWalletExportFormat : IWalletDataSource
{
    public AccountKeys? accountKeys { get; set; }
    public string? address { get; set; }

    public AccountAddress TryGetAddress()
    {
        AccountAddress accountAddress = AccountAddress.From(address);
        return accountAddress;
    }

    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetSignKeys()
    {
        return accountKeys.TryGetSignKeys();
    }
}
