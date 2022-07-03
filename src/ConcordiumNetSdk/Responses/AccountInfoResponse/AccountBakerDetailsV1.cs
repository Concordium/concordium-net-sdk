namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an account baker details version 1.
/// </summary>
public record AccountBakerDetailsV1 : AccountBakerDetails
{
    /// <summary>
    /// Gets or initiates the baker pool info.
    /// </summary>
    public BakerPoolInfo BakerPoolInfo { get; init; }
}
