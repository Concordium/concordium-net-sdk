namespace ConcordiumNetSdk.Responses.BirkParametersResponse;

/// <summary>
/// Represents the information about a parameters used for baking as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetBirkParametersAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/edit/main/docs/grpc.md#getbirkparameters--blockhash---birkparameters">here</a>.
/// </summary>
public record BirkParameters
{
    /// <summary>
    /// Gets or initiates the election difficulty for block election.
    /// </summary>
    public decimal ElectionDifficulty { get; init; }

    /// <summary>
    /// Gets or initiates the base16 encoded leadership election nonce.
    /// </summary>
    public string ElectionNonce { get; init; }

    /// <summary>
    /// Gets or initiates the bakers.
    /// </summary>
    public BakerInfo[] Bakers { get; init; }
}
