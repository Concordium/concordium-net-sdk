namespace Concordium.Sdk.Types;

/// <summary>
/// The network information of a node.
/// </summary>
/// <param name="NodeId">
/// An identifier which it uses to identify itself to other peers and it
/// is used for logging purposes internally. NB. The <see cref="NodeId"/> is spoofable
/// and as such should not serve as a trust instrument.
/// </param>
/// <param name="PeerTotalSent">The total amount of packets sent by the node.</param>
/// <param name="PeerTotalReceived">The total amount of packets received by the node.</param>
/// <param name="AvgBpsIn">The average bytes per second received by the node.</param>
/// <param name="AvgBpsOut">The average bytes per second transmitted by the node.</param>
public sealed record NetworkInfo(
    string NodeId,
    ulong PeerTotalSent,
    ulong PeerTotalReceived,
    ulong AvgBpsIn,
    ulong AvgBpsOut
)
{
    internal static NetworkInfo From(Grpc.V2.NodeInfo.Types.NetworkInfo info) =>
        new(
            info.NodeId.Value,
            info.PeerTotalSent,
            info.PeerTotalReceived,
            info.AvgBpsIn,
            info.AvgBpsOut
        );
}
