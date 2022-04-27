namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the pending releases on the account.
/// </summary>
public record AccountReleaseSchedule
{
    // todo: think of implementing value as type Amount look node sdk documentation
    /// <summary>
    /// Gets or initiates the total locked amount. 
    /// </summary>
    public string Total { get; init; }

    /// <summary>
    /// Gets or initiates the list of the scheduled releases in ascending timestamp order. 
    /// </summary>
    public List<ReleaseSchedule> Schedule { get; init; }
}
