using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the scheduled release.
/// </summary>
public class ReleaseSchedule
{
    /// <summary>
    /// Gets or initiates the time at which this amount is released in milliseconds since the UNIX epoch.
    /// </summary>
    public Timestamp Date { get; init; }

    // todo: think of implementing value as type Amount look node sdk documentation
    /// <summary>
    /// Gets or initiates the amount to be released.
    /// </summary>
    public long Amount { get; init; }

    // todo: think of implementing value as type TransactionHash look node sdk documentation
    /// <summary>
    /// Gets or initiates the list of the transaction hashes that contributed to this release.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Transactions { get; init; }
}
