using CommandLine;

namespace Concordium.Sdk.Examples.Common;

/// <summary>
/// Command line options for the runnable SDK examples.
///
/// This class can be used to specify options for the
/// <see cref="Client.ConcordiumClient"/>
/// at the command-line.
/// </summary>
public class ExampleOptions
{
    /// <summary>
    /// URL representing the endpoint where the GRPC V2 API is served.
    /// </summary>
    private const string DefaultEndpoint = "https://localhost/";

    /// <summary>
    /// Default port at the endpoint where the GRPC V2 API is served.
    /// </summary>
    private const ushort DefaultPort = 20000;

    [Option(
        'e',
        "endpoint",
        HelpText = "URL representing the endpoint where the GRPC V2 API is served.",
        Default = DefaultEndpoint
    )]
    public string Endpoint { get; set; } = DefaultEndpoint;

    [Option(
        'p',
        "port",
        HelpText = "Port at the endpoint where the GRPC V2 API is served.",
        Default = DefaultPort
    )]
    public ushort Port { get; set; } = DefaultPort;
}
