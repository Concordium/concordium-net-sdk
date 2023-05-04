using CommandLine;

namespace ConcordiumNetSdk.Examples;

public abstract class ExampleOptions
{
    const string DEFAULT_URL = "https://localhost/";
    const UInt16 DEFAULT_PORT = 20000;
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
