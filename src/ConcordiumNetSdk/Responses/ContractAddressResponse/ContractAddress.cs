using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.ContractAddressResponse;

/// <summary>
/// Represents the information about a contract instance as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetInstancesAsync"/>.
/// </summary>
public class ContractAddress
{
    /// <summary>
    /// Gets or initiates the index.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Gets or initiates the sub index.
    /// </summary>
    [JsonPropertyName("subindex")]
    public int SubIndex { get; init; }
}
