namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a commission rates.
/// </summary>
public record CommissionRates
{
    /// <summary>
    /// Gets or initiates the transaction commission.
    /// </summary>
    public int TransactionCommission { get; init; }

    /// <summary>
    /// Gets or initiates the baking commission.
    /// </summary>
    public int BakingCommission { get; init; }

    /// <summary>
    /// Gets or initiates the finalization commission.
    /// </summary>
    public int FinalizationCommission { get; init; }
}
