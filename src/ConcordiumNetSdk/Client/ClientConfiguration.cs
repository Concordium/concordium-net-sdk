namespace Concordium.Sdk.Client;

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
    /// Initializes a new instance of the <see cref="ClientConfiguration"/> class.
    /// <param name="timeout">The request timeout in seconds.</param>
    public ClientConfiguration(ulong timeout)
    {
        this.Timeout = timeout;
    }
}
