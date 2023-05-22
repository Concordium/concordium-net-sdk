using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// genesis wallet key export format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="JsonConvert"/>.
/// </summary>
internal class GenesisWalletExportFormat : IWalletDataSource
{
    [JsonProperty("accountKeys", Required = Required.DisallowNull)]
    internal AccountKeys? AccountKeysField { get; init; }

    [JsonProperty("address", Required = Required.DisallowNull)]
    internal string? AddressField { get; init; }

    public AccountAddress TryGetAccountAddress()
    {
        if (this.AddressField is null)
        {
            throw new WalletDataSourceException("Required field 'address' is missing.");
        }
        try
        {
            return AccountAddress.From(this.AddressField);
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
        if (this.AccountKeysField is null)
        {
            throw new WalletDataSourceException("Required field 'accountKeys' is missing.");
        }
        try
        {
            return this.AccountKeysField.TryGetSignKeys();
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
