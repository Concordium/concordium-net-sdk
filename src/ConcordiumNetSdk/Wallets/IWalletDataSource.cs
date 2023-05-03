using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Crypto;

namespace ConcordiumNetSdk.Wallets;

/// <summary>
/// Implementers of this interface corresponds to a data source
/// from which a <see cref="WalletAccount"/> can be constructed.
///
/// Specifically this allows for retrieving sign key and wallet
/// address data in a failing manner.
/// </summary>
public interface IWalletDataSource
{
    /// <summary>
    /// Try to retrieve the sign keys from the wallet data source.
    /// </summary>
    public Dictionary<
        AccountCredentialIndex,
        Dictionary<AccountKeyIndex, ISigner>
    > TryGetSignKeys();

    /// <summary>
    /// Try to retrieve the wallet address from the wallet data source.
    /// </summary>
    public AccountAddress TryGetAddress();
}
