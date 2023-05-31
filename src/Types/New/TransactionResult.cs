namespace Concordium.Sdk.Types.New;

public abstract class TransactionResult
{
    public abstract IEnumerable<AccountAddress> GetAccountAddresses();
    public abstract IEnumerable<ulong> GetBakerIds();

    public virtual IEnumerable<AccountBalanceUpdate> GetAccountBalanceUpdates(TransactionSummary owningTransaction)
    {
        return Enumerable.Empty<AccountBalanceUpdate>();
    }
}