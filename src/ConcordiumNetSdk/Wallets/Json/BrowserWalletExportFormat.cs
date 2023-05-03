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

    public AccountAddress TryGetAccountAddress()
    {
        try
        {
            if (value is null)
            {
                throw new ArgumentNullException("Required field 'value' is null.");
            }
            if (value.address is null)
            {
                throw new ArgumentNullException("Required field 'address' is null.");
            }
            return AccountAddress.From(value.address);
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
            if (value is null)
            {
                throw new ArgumentNullException("Required field 'value' is null.");
            }
            if (value.accountKeys is null)
            {
                throw new ArgumentNullException("Required field 'accountKeys' is null.");
            }
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
