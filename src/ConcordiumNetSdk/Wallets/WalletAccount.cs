using System.Collections.Immutable;

using Newtonsoft.Json;

using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Transactions;

namespace ConcordiumNetSdk.Wallets;

/// <summary>
/// Represents an account imported from one of the supported wallet export formats.
///
/// In particular it supports import of the browser and genesis wallet key export
/// formats. The class implements <see cref="ITransactionSigner"/> so it may be used
/// for signing transactions.
///
/// This structure does not have the encryption key for sending encrypted transfers, it only
/// contains keys for signing transactions.
/// </summary>
public class WalletAccount : ITransactionSigner
{
    /// <summary>
    /// The address of the imported account.
    /// </summary>
    public readonly AccountAddress AccountAddress;

    /// <summary>
    /// Internal representation of a signer.
    /// </summary>
    private TransactionSigner _signer;

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletAccount"/> class.
    /// </summary>
    /// <param name="accountAddress">The address of the imported account.</param>
    /// <param name="signKeys">The signers corresponding to the keys of the imported account.</param>
    private WalletAccount(
        AccountAddress accountAddress,
        Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> signKeys
    )
    {
        AccountAddress = accountAddress;
        _signer = new TransactionSigner();
        signKeys
            .ToList()
            .ForEach(cred =>
            {
                cred.Value
                    .ToList()
                    .ForEach(key =>
                    {
                        _signer.AddSignerEntry(cred.Key, key.Key, key.Value);
                    });
            });
    }

    /// <summary>
    /// Try to create a new instance from a wallet data source.
    /// </summary>
    /// <param name="json">JSON string in the genesis wallet key export format.</param>
    public static WalletAccount From(IWalletDataSource importedWallet)
    {
        return new WalletAccount(
            importedWallet.TryGetAccountAddress(),
            importedWallet.TryGetSignKeys()
        );
    }

    /// <summary>
    /// Try to create a new instance from a JSON string in the genesis wallet key export format.
    /// </summary>
    /// <param name="json">JSON string in the genesis wallet key export format.</param>
    /// <exception cref="JsonException">The specified input does not contain valid JSON.</exception>
    /// <exception cref="WalletDataSourceException">Either a field is missing or an index or sign key could not be parsed.</exception>
    public static WalletAccount FromGenesisWalletExportFormat(string json)
    {
        Json.GenesisWalletExportFormat genesisWallet =
            JsonConvert.DeserializeObject<Json.GenesisWalletExportFormat>(json);
        return From(genesisWallet);
    }

    /// <summary>
    /// Try to create a new instance from a JSON string in the browser wallet key export format.
    /// </summary>
    /// <param name="json">JSON string in the browser wallet key export format.</param>
    /// <exception cref="JsonException">The specified input does not contain valid JSON.</exception>
    /// <exception cref="WalletDataSourceException">Either a field is missing or an index or sign key could not be parsed.</exception>
    public static WalletAccount FromBrowserWalletExportFormat(string json)
    {
        Json.BrowserWalletExportFormat genesisWallet =
            JsonConvert.DeserializeObject<Json.BrowserWalletExportFormat>(json);
        return From(genesisWallet);
    }

    public ImmutableDictionary<
        AccountCredentialIndex,
        ImmutableDictionary<AccountKeyIndex, ISigner>
    > GetSignerEntries()
    {
        return _signer.GetSignerEntries();
    }

    public byte GetSignatureCount()
    {
        return _signer.GetSignatureCount();
    }

    public AccountTransactionSignature Sign(byte[] data)
    {
        return _signer.Sign(data);
    }
}
