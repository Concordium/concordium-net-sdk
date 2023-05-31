namespace Concordium.Sdk.Types.New;

public abstract class SpecialEvent
{
    public abstract IEnumerable<AccountBalanceUpdate> GetAccountBalanceUpdates();
}