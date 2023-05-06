using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Crypto;

namespace Concordium.Sdk.Wallets;

/// <summary>
/// Implementers of this interface corresponds to a data source
/// from which a <see cref="WalletAccount"/> can be constructed.
///
/// Specifically this allows for retrieving sign key and wallet
/// address data in a manner which is subject to failure.
/// </summary>
public interface IWalletDataSource
{
    /// <summary>
    /// Try to retrieve the sign keys from the wallet data source.
    /// </summary>
    /// <exception cref="WalletDataSourceException">
    /// An error condition occurred while trying to get the sign keys.
    /// </exception>
    public Dictionary<
        AccountCredentialIndex,
        Dictionary<AccountKeyIndex, ISigner>
    > TryGetSignKeys();

    /// <summary>
    /// Try to retrieve the wallet address from the wallet data source.
    /// </summary>
    /// <exception cref="WalletDataSourceException">
    /// An error condition occurred while trying to get the account address.
    /// </exception>
    public AccountAddress TryGetAccountAddress();
}
