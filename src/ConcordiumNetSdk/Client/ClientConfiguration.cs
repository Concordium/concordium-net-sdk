namespace ConcordiumNetSdk.Client;

/// <summary>
/// Specifies connection settings for the <see cref="Client"/>.
/// </summary>
public class ClientConfiguration
{
    /// <summary>
    /// The request timeout in seconds.
    /// </summary>
    public ulong Timeout { get; private set; }

    /// <summary>
    /// Flag indicating whether the client must use a secure connection.
    /// </summary>
    public bool Secure { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientConfiguration"/> class.
    /// <param name="timeout">The request timeout in seconds.</param>
    /// <param name="secure">Flag indicating whether the client must use a secure connection.</param>
    public ClientConfiguration(ulong timeout, bool secure)
    {
        this.Timeout = timeout;
        this.Secure = secure;
    }
}
