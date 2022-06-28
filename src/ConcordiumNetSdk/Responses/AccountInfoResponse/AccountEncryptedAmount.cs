namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the current encrypted balance of the account.
/// </summary>
public record AccountEncryptedAmount
{
    /// <summary>
    /// Gets or initiates the encrypted amount that is the result of actions of the account, i.e., shielding, unshielding, and sending encrypted transfers.
    /// </summary>
    public string SelfAmount { get; init; }

    /// <summary>
    /// Gets or initiates  the starting index for amounts in incomingAmounts.
    /// This is needed when sending encrypted transfers to indicate
    /// which amounts from the list of incomingAmounts have been used as input to the encrypted transfer. 
    /// </summary>
    public ulong StartIndex { get; init; }

    /// <summary>
    /// Gets or initiates amounts that were sent to this account by other accounts.
    /// </summary>
    public List<string> IncomingAmounts { get; init; }
}
