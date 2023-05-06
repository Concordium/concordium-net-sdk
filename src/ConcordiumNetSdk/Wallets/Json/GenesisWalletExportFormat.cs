using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;

using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// genesis wallet key export format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
internal record GenesisWalletExportFormat : IWalletDataSource
{
    [JsonProperty(Required = Required.DisallowNull)]
    internal AccountKeys accountKeys { get; init; }

    [JsonProperty(Required = Required.DisallowNull)]
    internal string address { get; init; }

    public AccountAddress TryGetAccountAddress()
    {
        try
        {
            return AccountAddress.From(address);
        }
        catch (Exception e)
        {
            throw new WalletDataSourceException(
                "Could not parse the account address from the supplied genesis wallet key export format data.",
                e
            );
        }
    }

    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetSignKeys()
    {
        try
        {
            return accountKeys.TryGetSignKeys();
        }
        catch (Exception e)
        {
            throw new WalletDataSourceException(
                "Could not parse the sign keys from the supplied genesis wallet key export format data.",
                e
            );
        }
    }
}
