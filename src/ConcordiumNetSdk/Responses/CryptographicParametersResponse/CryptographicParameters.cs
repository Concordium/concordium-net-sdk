namespace ConcordiumNetSdk.Responses.CryptographicParametersResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a cryptographic parameters in a specific block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetCryptographicParametersAsync"/>.
/// </summary>
public record CryptographicParameters 
{
    /// <summary>
    /// Gets or initiates the on chain commitment key.
    /// </summary>
    public string OnChainCommitmentKey { get; init; }

    /// <summary>
    /// Gets or initiates the bulletproof generators.
    /// </summary>
    public string BulletproofGenerators { get; init; }

    /// <summary>
    /// Gets or initiates the genesis string.
    /// </summary>
    public string GenesisString { get; init; }
}
