using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Crypto;

public interface IImportedWallet
{
    public Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, ISigner>> TryGetKeys();
    public AccountAddress TryGetAddress();
}
