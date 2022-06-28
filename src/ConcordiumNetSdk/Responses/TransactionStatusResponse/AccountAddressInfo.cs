using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about an account address.
/// </summary>
public record AccountAddressInfo
{
    /// <summary>
    /// Gets or initiates the account address type.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets or initiates the account address.
    /// </summary>
    public AccountAddress Address { get; init; }
}
