using CommandLine;

namespace Concordium.Sdk.Examples;

/// <summary>
/// Command line options for the runnable SDK examples.
///
/// This class can be used to specify options for the
/// <see cref="Concordium.Sdk.Client.ConcordiumClient"/>
/// at the command-line.
/// </summary>
public class ExampleOptions
{
    /// <summary>
    /// URL representing the endpoint where the GRPC V2 API is served.
    /// </summary>
    const string DEFAULT_ENDPOINT = "https://localhost/";

    /// <summary>
    /// Default port at the endpoint where the GRPC V2 API is served.
    /// </summary>
    const UInt16 DEFAULT_PORT = 20000;

    [Option(
        'e',
        "endpoint",
        HelpText = "URL representing the endpoint where the GRPC V2 API is served.",
        Default = DEFAULT_ENDPOINT
    )]
    public string Endpoint { get; set; } = DEFAULT_ENDPOINT;

    [Option(
        'p',
        "port",
        HelpText = "Port at the endpoint where the GRPC V2 API is served.",
        Default = DEFAULT_PORT
    )]
    public UInt16 Port { get; set; } = DEFAULT_PORT;
}
