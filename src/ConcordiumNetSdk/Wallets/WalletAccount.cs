using System.Collections.Immutable;

using Newtonsoft.Json;

using ConcordiumNetSdk.Crypto;
using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Transactions;

namespace ConcordiumNetSdk.Wallets;

public class WalletAccount : ITransactionSigner
{
    private AccountAddress _accountAddress;
    private TransactionSigner _signer;

    private WalletAccount(
        AccountAddress accountAddress,
        Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> signKeys
    )
    {
        this._accountAddress = accountAddress;
        this._signer = new TransactionSigner();
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

    public AccountAddress GetAccountAddress()
    {
        return (AccountAddress)_accountAddress;
    }

    public ImmutableDictionary<
        AccountCredentialIndex,
        ImmutableDictionary<AccountKeyIndex, ISigner>
    > AccountKeys()
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

    public static WalletAccount From(IImportedWallet importedWallet)
    {
        return new WalletAccount(importedWallet.TryGetAddress(), importedWallet.TryGetKeys());
    }

    public static WalletAccount FromGenesisWalletExportFormat(string json)
    {
        Helpers.Json.GenesisExportFormatWallet genesisWallet =
            JsonConvert.DeserializeObject<Helpers.Json.GenesisExportFormatWallet>(json);
        return From(genesisWallet);
    }
}
