namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a release schedule with transactions.
/// </summary>
public record ReleaseScheduleWithTransactions : ReleaseSchedule
{
    /// <summary>
    /// Gets or initiates the transactions.
    /// </summary>
    public List<string> Transactions { get; init; }
}
