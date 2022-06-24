using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for updates.
/// </summary>
public abstract record Updates
{
    /// <summary>
    /// Gets or initiates the protocol update.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProtocolUpdate? ProtocolUpdate { get; init; }
}
