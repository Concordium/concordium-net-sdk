using Grpc.Net.Client;

namespace Concordium.Sdk.Client;

/// <summary>
/// Configurations to Concordium Client.
/// </summary>
public sealed class ConcordiumClientOptions
{
    /// <summary>
    /// Endpoint of a resource where the V2 API is served.
    /// </summary>
    public Uri Endpoint { get; init; }
    /// <summary>
    /// The maximum permitted duration of a call made by this client.
    /// <c>null</c> allows the call to run indefinitely.
    /// </summary>
    public TimeSpan? Timeout { get; init; }
    /// <summary>
    /// Optionally to specify connection settings such as the retry policy or keepalive ping.
    ///
    /// By default the policy is not to retry if a connection could not be established.
    ///
    /// See https://github.com/grpc/grpc/blob/master/doc/keepalive.md for default values
    /// for the keepalive ping parameters.
    /// </summary>
    public GrpcChannelOptions? ChannelOptions { get; init; }
}
