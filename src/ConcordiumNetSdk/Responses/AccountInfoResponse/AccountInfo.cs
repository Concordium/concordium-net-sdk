using System.Text.Json.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the state of an account in the given block as response data of <see cref="ConcordiumNodeClient"/>.<see cref="ConcordiumNodeClient.GetAccountInfoAsync"/>.
/// </summary>
public abstract record AccountInfo
{
    /// <summary>
    /// Gets or initiates the sequential index of the account (in the order of creation). If the account is a baker this index is used as the bakerId.
    /// </summary>
    [JsonPropertyName("accountIndex")]
    public ulong Index { get; init; }

    /// <summary>
    /// Gets or initiates the next available nonce on this account at this block.
    /// </summary>
    [JsonPropertyName("accountNonce")]
    public ulong Nonce { get; init; }

    /// <summary>
    /// Gets or initiates the current public account balance
    /// </summary>
    [JsonPropertyName("accountAmount")]
    public string Amount { get; init; }

    /// <summary>
    /// Gets or initiates the Elgamal public key used to send encrypted transfers to the account.
    /// </summary>
    [JsonPropertyName("accountEncryptionKey")]
    public string EncryptionKey { get; init; }

    /// <summary>
    /// Gets or initiates the non-zero positive integer that specifies how many credentials must sign any transaction from the account.
    /// </summary>
    [JsonPropertyName("accountThreshold")]
    public int Threshold { get; init; }

    /// <summary>
    /// Gets or initiates the account credential values deployed on the account.
    /// </summary>
    [JsonPropertyName("accountCredentials")]
    public Dictionary<byte, VersionedValue<AccountCredential>> Credentials { get; init; }

    /// <summary>
    /// Gets  or initiates the current encrypted balance of the account.
    /// </summary>
    [JsonPropertyName("accountEncryptedAmount")]
    public AccountEncryptedAmount EncryptedAmount { get; init; }

    /// <summary>
    /// Gets or initiates the pending releases on the account.
    /// </summary>
    [JsonPropertyName("accountReleaseSchedule")]
    public AccountReleaseSchedule ReleaseSchedule { get; init; }
}
