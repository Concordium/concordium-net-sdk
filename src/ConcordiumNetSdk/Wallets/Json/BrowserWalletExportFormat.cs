using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// browser wallet key export format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
public class BrowserWalletExportFormat : IWalletDataSource
{
    public Value? value { get; set; }

    public AccountAddress TryGetAddress()
    {
        AccountAddress accountAddress = AccountAddress.From(value.address);
        return accountAddress;
    }

    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetSignKeys()
    {
        return value.accountKeys.TryGetSignKeys();
    }
}
