using Concordium.Sdk.Crypto;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object which is in the
/// genesis wallet key export format.
/// </summary>
public record GenesisWalletExportFormat : IWalletDataSource
{
    public AccountKeys AccountKeys { get; init; }

    public string Address { get; init; }

    public AccountAddress TryGetAccountAddress()
    {
        if (this.Address is null)
        {
            throw new WalletDataSourceException("Required field 'address' is missing.");
        }
        try
        {
            return AccountAddress.From(this.Address);
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
        if (this.AccountKeys is null)
        {
            throw new WalletDataSourceException("Required field 'accountKeys' is missing.");
        }
        try
        {
            return this.AccountKeys.TryGetSignKeys();
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
