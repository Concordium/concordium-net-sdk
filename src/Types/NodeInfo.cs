using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// The status of the requested node.
/// </summary>
/// <param name="Version">The version of the node.</param>
/// <param name="LocalTime">The local (UTC) time of the node.</param>
/// <param name="UpTime">How long the node has been alive.</param>
/// <param name="NetworkInfo">Information related to the network for the node.</param>
/// <param name="Details">Information related to consensus for the node.</param>
public sealed record NodeInfo(
    PeerVersion Version,
    DateTimeOffset LocalTime,
    TimeSpan UpTime,
    NetworkInfo NetworkInfo,
    INodeDetails Details)
{
    internal static NodeInfo From(Grpc.V2.NodeInfo nodeInfo) =>
        new(
            PeerVersion.Parse(nodeInfo.PeerVersion),
            nodeInfo.LocalTime.ToDateTimeOffset(),
            TimeSpan.FromMilliseconds(nodeInfo.PeerUptime.Value),
            NetworkInfo.From(nodeInfo.NetworkInfo),
            NodeDetailsFactory.From(nodeInfo)
        );
}
