namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about an authorizations version 1.
/// </summary>
public record AuthorizationsV1 : Authorizations
{
    /// <summary>
    /// Gets or initiates the cooldown parameters.
    /// </summary>
    public Authorization CooldownParameters { get; init; }

    /// <summary>
    /// Gets or initiates the time parameters.
    /// </summary>
    public Authorization TimeParameters { get; init; }
}
