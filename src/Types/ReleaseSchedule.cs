namespace Concordium.Sdk.Types;

/// <summary>
/// State of the account's release schedule. This is the balance of the account
/// that is owned by the account, but cannot be used until the release point.
/// </summary>
/// <param name="Total">Total amount locked in the release schedule.</param>
/// <param name="Schedules">A list of releases, ordered by increasing timestamp.</param>
public sealed record ReleaseSchedule(
    CcdAmount Total,
    IList<Release> Schedules
)
{
    internal static ReleaseSchedule From(Grpc.V2.ReleaseSchedule schedule) => new(
        CcdAmount.From(schedule.Total),
        schedule.Schedules.Select(Release.From).ToList()
    );
}
