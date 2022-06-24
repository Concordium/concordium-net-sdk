using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about an account address.
/// </summary>
public class AccountAddressInfo
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
