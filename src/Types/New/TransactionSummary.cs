namespace Concordium.Sdk.Types.New;

public class TransactionSummary
{
    public TransactionSummary(AccountAddress? sender, TransactionHash hash, CcdAmount cost, int energyCost, TransactionType type, TransactionResult result, int index)
    {
        this.Sender = sender;
        this.Hash = hash ?? throw new ArgumentNullException(nameof(hash));
        this.Cost = cost;
        this.EnergyCost = energyCost;
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
        this.Result = result ?? throw new ArgumentNullException(nameof(result));
        this.Index = index;
    }

    public AccountAddress? Sender { get;  }
    public TransactionHash Hash { get; } 
    public CcdAmount Cost { get; }  
    public int EnergyCost { get; }  
    public TransactionType Type { get; }
    public TransactionResult Result { get; }
    
    /// <summary>
    /// The index of the transaction in the block (0 based).
    /// </summary>
    public int Index { get; }

    public IEnumerable<AccountBalanceUpdate> GetAccountBalanceUpdates()
    {
        if (this.Sender != null && this.Cost > CcdAmount.FromMicroCcd(0))
            yield return new AccountBalanceUpdate(this.Sender.Value, -1 * (long)this.Cost.Value, BalanceUpdateType.TransactionFee, this.Hash);

        foreach (var balanceUpdate in this.Result.GetAccountBalanceUpdates(this))
            yield return balanceUpdate with { TransactionHash = this.Hash};
    }
}
