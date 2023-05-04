using CommandLine;

namespace ConcordiumNetSdk.Examples;

/// <summary>
/// Command line options for the runnable SDK examples.
///
/// This class can be used to specify options for the
/// <see cref="ConcordiumNetSdk.Client.ConcordiumClient"/>
/// at the command-line.
/// </summary>
public abstract class ExampleOptions
{
    /// <summary>
    /// Default URL representing the endpoint where the GRPC V2 API is served.
    /// </summary>
    const string DEFAULT_URL = "localhost";

    /// <summary>
    /// Default port at the endpoint where the GRPC V2 API is served.
    /// </summary>
    const UInt16 DEFAULT_PORT = 20000;

    /// <summary>
    /// Default flag representing whether a secure connection should be used.
    /// </summary>
    const bool DEFAULT_SECURE_CONNECTION_FLAG = true;

    [Option(
        'e',
        "endpoint",
        HelpText = "URL representing the endpoint where the GRPC V2 API is served.",
        Default = DEFAULT_URL
    )]
    public string Endpoint { get; set; } = DEFAULT_URL;

    [Option(
        'p',
        "port",
        HelpText = "Port at the endpoint where the GRPC V2 API is served.",
        Default = DEFAULT_PORT
    )]
    public UInt16 Port { get; set; } = DEFAULT_PORT;

    [Option(
        's',
        "secure",
        HelpText = "Flag representing whether a secure connection should be used.",
        Default = DEFAULT_SECURE_CONNECTION_FLAG
    )]
    public bool Secure { get; set; } = DEFAULT_SECURE_CONNECTION_FLAG;
}
