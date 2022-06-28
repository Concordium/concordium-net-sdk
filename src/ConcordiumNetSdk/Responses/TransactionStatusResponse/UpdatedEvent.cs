using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a updated event.
/// </summary>
public record UpdatedEvent : Event
{
    /// <summary>
    /// Gets or initiates the tag.
    /// </summary>
    public string Tag { get; init; }

    /// <summary>
    /// Gets or initiates the contract address.
    /// </summary>
    public ContractAddress Address { get; init; }

    /// <summary>
    /// Gets or initiates the instigator.
    /// </summary>
    public AccountAddressInfo Instigator { get; init; }

    /// <summary>
    /// Gets or initiates the amount.
    /// </summary>
    public ulong Amount { get; init; }

    /// <summary>
    /// Gets or initiates the message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets or initiates the events.
    /// </summary>
    public List<string> Events { get; init; }
}
