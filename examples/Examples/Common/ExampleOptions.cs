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
    /// URL representing the endpoint where the gRPC V2 API is served.
    /// </summary>
    public const string DefaultEndpoint = "https://localhost/";

    /// <summary>
    /// Default port at the endpoint where the gRPC V2 API is served.
    /// </summary>
    public const ushort DefaultPort = 20000;

    /// <summary>
    /// Default connection timeout in seconds.
    /// </summary>
    public const ushort DefaultTimeout = 60;

    [Option(
        'e',
        "endpoint",
        HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = DefaultEndpoint
    )]
    public string Endpoint { get; set; } = default!;

    [Option(
        'p',
        "port",
        HelpText = "Port at the endpoint where the gRPC V2 API is served.",
        Default = DefaultPort
    )]
    public ushort Port { get; set; } = default!;

    [Option(
        't',
        "timeout",
        HelpText = "Default connection timeout in seconds.",
        Default = DefaultTimeout
    )]
    public uint Timeout { get; set; } = default!;
}
