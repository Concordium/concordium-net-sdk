using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// browser wallet key export format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
internal record BrowserWalletExportFormat : IWalletDataSource
{
    internal record Value(AccountKeys accountKeys, string address)
    {
        [JsonProperty(Required = Required.DisallowNull)]
        internal AccountKeys accountKeys { get; init; } = accountKeys;

        [JsonProperty(Required = Required.DisallowNull)]
        internal string address { get; init; } = address;
    }

    [JsonProperty(Required = Required.DisallowNull)]
    internal Value value { get; init; }

    public AccountAddress TryGetAccountAddress()
    {
        try
        {
            return AccountAddress.From(value.address);
        }
        catch (Exception e)
        {
            throw new WalletDataSourceException(
                "Could not parse the account address from the supplied browser wallet key export format data.",
                e
            );
        }
    }

    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetSignKeys()
    {
        try
        {
            return value.accountKeys.TryGetSignKeys();
        }
        catch (Exception e)
        {
            throw new WalletDataSourceException(
                "Could not parse the sign keys from the supplied browser wallet key export format data.",
                e
            );
        }
    }
}
