using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// genesis wallet key export format.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
public class GenesisWalletExportFormat : IWalletDataSource
{
    public AccountKeys? accountKeys { get; set; }
    public string? address { get; set; }

    public AccountAddress TryGetAccountAddress()
    {
        try
        {
            if (address is null)
            {
                throw new ArgumentNullException("Field 'value' is null.");
            }
            AccountAddress accountAddress = AccountAddress.From(address);
            return accountAddress;
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
            if (accountKeys is null)
            {
                throw new ArgumentNullException("Required field 'accountKeys' is null.");
            }
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
