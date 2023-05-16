using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// browser wallet key export format.
///
/// </summary>
public record BrowserWalletExportFormat : IWalletDataSource
{
    public record ValueField
    {
        public AccountKeys AccountKeys { get; init; }

        public string Address { get; init; }
    }

    public ValueField Value { get; init; }

    public AccountAddress TryGetAccountAddress()
    {
        if (this.Value is null)
        {
            throw new WalletDataSourceException("Required field 'value' is missing.");
        }
        if (this.Value.Address is null)
        {
            throw new WalletDataSourceException("Required field 'address' is missing.");
        }
        try
        {
            return AccountAddress.From(this.Value.Address);
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
        if (this.Value is null)
        {
            throw new WalletDataSourceException("Required field 'value' is missing.");
        }
        if (this.Value.AccountKeys is null)
        {
            throw new WalletDataSourceException("Required field 'accountKeys' is missing.");
        }
        try
        {
            return this.Value.AccountKeys.TryGetSignKeys();
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
