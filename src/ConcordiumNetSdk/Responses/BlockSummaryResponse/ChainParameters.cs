using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for chain parameters.
/// </summary>
public abstract record ChainParameters
{
    /// <summary>
    /// Gets or initiates the election difficulty.
    /// </summary>
    public decimal ElectionDifficulty { get; init; }

    /// <summary>
    /// Gets or initiates the euro per energy.
    /// </summary>
    public ExchangeRate EuroPerEnergy { get; init; }

    /// <summary>
    /// Gets or initiates the micro gtu per euro.
    /// </summary>
    [JsonPropertyName("microGTUPerEuro")]
    public ExchangeRate MicroGtuPerEuro { get; init; }

    /// <summary>
    /// Gets or initiates the account creation limit.
    /// </summary>
    public int AccountCreationLimit { get; init; }

    /// <summary>
    /// Gets or initiates the foundation account index.
    /// </summary>
    public ulong FoundationAccountIndex { get; init; }
}
