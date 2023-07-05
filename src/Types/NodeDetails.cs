using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;


/// <summary>
/// Consensus related information for a node.
/// </summary>
public interface INodeDetails
{ }

internal static class NodeDetailsFactory
{
    internal static INodeDetails From(Grpc.V2.NodeInfo nodeInfo)
    {
        return nodeInfo.DetailsCase switch
        {
            Grpc.V2.NodeInfo.DetailsOneofCase.Bootstrapper => new Bootstrapper(),
            Grpc.V2.NodeInfo.DetailsOneofCase.Node => Node.From(nodeInfo.Node),
            _ => throw new MissingEnumException<Grpc.V2.NodeInfo.DetailsOneofCase>(nodeInfo.DetailsCase),
        };
    }
}

/// <summary>
/// The node is a bootstrapper and does not
/// run the consensus protocol.
/// </summary>
public sealed record Bootstrapper : INodeDetails;

/// <summary>
/// The node is a regular node and is eligible for
/// running the consensus protocol.
/// </summary>
public sealed record Node(INodeConsensusStatus Status) : INodeDetails
{
    internal static Node From(Grpc.V2.NodeInfo.Types.Node node) => new(NodeConsensusStatusFactory.From(node));
}
