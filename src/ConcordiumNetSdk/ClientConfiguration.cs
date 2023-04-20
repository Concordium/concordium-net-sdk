namespace ConcordiumNetSdk;

/// <summary>
/// Specifies various settings for the <see cref="Client"/>.
/// </summary>
public class ClientConfiguration
{
    /// <summary>
    /// The default request timeout in seconds.
    /// </summary>
    public const ulong DEFAULT_TIMEOUT = 30;

    /// <summary>
    /// Default flag value indicating whether the client must use a secure connection.
    /// </summary>
    public const bool DEFAULT_SECURE_FLAG = true;

    /// <summary>
    /// The request timeout in seconds.
    /// </summary>
    public ulong Timeout { get; private set; }

    /// <summary>
    /// Flag indicating whether the client must use a secure connection.
    /// Note that the URL must specify <c>https://</c> accordingly.
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

    /// <summary>
    /// Get the default configuration, instructing the client to use a request timeout of 30 seconds and a secure connection.
    /// </summary>
    public static ClientConfiguration DefaultConfiguration =>
        new ClientConfiguration(DEFAULT_TIMEOUT, DEFAULT_SECURE_FLAG);
}
