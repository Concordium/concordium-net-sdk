using System.Collections.Immutable;
using Concordium.Sdk.Crypto;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Newtonsoft.Json;

namespace Concordium.Sdk.Wallets;

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
    public AccountAddress AccountAddress { get; init; }

    /// <summary>
    /// Internal representation of a signer.
    /// </summary>
    private readonly TransactionSigner _signer;

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
        this.AccountAddress = accountAddress;
        this._signer = new TransactionSigner();
        foreach (var (key, value) in signKeys)
        {
            foreach (var (accountKeyIndex, signer) in value)
            {
                this._signer.AddSignerEntry(key, accountKeyIndex, signer);
            }
        }
    }

    /// <summary>
    /// Try to create a new instance from a wallet data source.
    /// </summary>
    /// <param name="walletDataSource">Data source from which to import the wallet.</param>
    /// <exception cref="WalletDataSourceException"/>
    public static WalletAccount From(IWalletDataSource walletDataSource) =>
        new(walletDataSource.TryGetAccountAddress(), walletDataSource.TryGetSignKeys());

    /// <summary>
    /// Create a new instance from a string in the browser or genesis wallet key export format.
    /// </summary>
    /// <param name="json">JSON string in the browser or genesis wallet key export format.</param>
    /// <exception cref="JsonException">The specified input is not valid JSON.</exception>
    /// <exception cref="WalletDataSourceException">Either a field is missing or an index or sign key could not be parsed.</exception>
    public static WalletAccount FromWalletKeyExportFormat(string json)
    {
        try
        {
            return FromGenesisWalletKeyExportFormat(json);
        }
        catch (Exception e1) when (e1 is JsonException or WalletDataSourceException)
        {
            try
            {
                return FromBrowserWalletKeyExportFormat(json);
            }
            catch (Exception e2) when (e2 is JsonException or WalletDataSourceException)
            {
                throw e1;
            }
        }
    }

    /// <summary>
    /// Try to create a new instance from a JSON string in the genesis wallet key export format.
    /// </summary>
    /// <param name="json">JSON string in the genesis wallet key export format.</param>
    /// <exception cref="JsonException">The specified input is not valid JSON.</exception>
    /// <exception cref="WalletDataSourceException">Either a field is missing or an index or sign key could not be parsed.</exception>
    private static WalletAccount FromGenesisWalletKeyExportFormat(string json)
    {
        var genesisWallet =
            JsonConvert.DeserializeObject<Json.GenesisWalletExportFormat>(json)
            ?? throw new WalletDataSourceException(
                "Call to DeserializeObject did not return a value."
            );
        return From(genesisWallet);
    }

    /// <summary>
    /// Try to create a new instance from a JSON string in the browser wallet key export format.
    /// </summary>
    /// <param name="json">JSON string in the browser wallet key export format.</param>
    /// <exception cref="JsonException">The specified input is not valid JSON.</exception>
    /// <exception cref="WalletDataSourceException">Either a field is missing or an index or sign key could not be parsed.</exception>
    private static WalletAccount FromBrowserWalletKeyExportFormat(string json)
    {
        var genesisWallet =
            JsonConvert.DeserializeObject<Json.BrowserWalletExportFormat>(json)
            ?? throw new WalletDataSourceException(
                "Call to DeserializeObject did not return a value."
            );
        return From(genesisWallet);
    }

    /// <summary> Get credential-key-map of the account.</summary>
    public ImmutableDictionary<
        AccountCredentialIndex,
        ImmutableDictionary<AccountKeyIndex, ISigner>
    > GetSignerEntries() => this._signer.GetSignerEntries();

    /// <summary> Get the number of signatures included in the AccountSignature when signing using this account.</summary>
    public byte GetSignatureCount() => this._signer.GetSignatureCount();

    /// <summary> Produce an AccountTransactionSignature using this account. </summary>
    public AccountTransactionSignature Sign(byte[] data) => this._signer.Sign(data);
}
