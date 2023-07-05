using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Details of the consensus protocol running on the node.
/// </summary>
public interface INodeConsensusStatus {}

internal static class NodeConsensusStatusFactory
{
    internal static INodeConsensusStatus From(Grpc.V2.NodeInfo.Types.Node node)
    {
        switch (node.ConsensusStatusCase)
        {
            case Grpc.V2.NodeInfo.Types.Node.ConsensusStatusOneofCase.NotRunning:
                return new ConsensusNotRunning();
            case Grpc.V2.NodeInfo.Types.Node.ConsensusStatusOneofCase.Passive:
                return new ConsensusPassive();
            case Grpc.V2.NodeInfo.Types.Node.ConsensusStatusOneofCase.Active:
                var bakerId = BakerId.From(node.Active.BakerId);
                switch (node.Active.StatusCase)
                {
                    case Grpc.V2.NodeInfo.Types.BakerConsensusInfo.StatusOneofCase.PassiveCommitteeInfo:
                        return node.Active.PassiveCommitteeInfo switch
                        {
                            Grpc.V2.NodeInfo.Types.BakerConsensusInfo.Types.PassiveCommitteeInfo.NotInCommittee =>
                                new NotInCommittee(bakerId),
                            Grpc.V2.NodeInfo.Types.BakerConsensusInfo.Types.PassiveCommitteeInfo
                                .AddedButNotActiveInCommittee => new AddedButNotActiveInCommittee(bakerId),
                            Grpc.V2.NodeInfo.Types.BakerConsensusInfo.Types.PassiveCommitteeInfo.AddedButWrongKeys =>
                                new AddedButWrongKeys(bakerId),
                            _ => throw new
                                MissingEnumException<
                                    Grpc.V2.NodeInfo.Types.BakerConsensusInfo.Types.PassiveCommitteeInfo>(node.Active
                                    .PassiveCommitteeInfo)
                        };
                    case Grpc.V2.NodeInfo.Types.BakerConsensusInfo.StatusOneofCase.ActiveBakerCommitteeInfo:
                        return new Baker(bakerId);
                    case Grpc.V2.NodeInfo.Types.BakerConsensusInfo.StatusOneofCase.ActiveFinalizerCommitteeInfo:
                        return new Finalizer(bakerId);
                    case Grpc.V2.NodeInfo.Types.BakerConsensusInfo.StatusOneofCase.None:
                    default:
                        throw new MissingEnumException<Grpc.V2.NodeInfo.Types.BakerConsensusInfo.StatusOneofCase>(
                            node.Active.StatusCase);
                }
            case Grpc.V2.NodeInfo.Types.Node.ConsensusStatusOneofCase.None:
            default:
                throw new MissingEnumException<Grpc.V2.NodeInfo.Types.Node.ConsensusStatusOneofCase>(
                    node.ConsensusStatusCase);
        }
    }
}

/// <summary>
/// The consensus protocol is not running on the node.
/// This only occurs when the node does not support the protocol on the
/// chain or the node is a 'Bootstrapper'.
/// </summary>
public sealed record ConsensusNotRunning : INodeConsensusStatus;

/// <summary>
/// The node is a passive member of the consensus. This means:
/// * The node is processing blocks.
/// * The node is relaying transactions and blocks onto the network.
/// * The node is responding to catch up messages from its peers.
/// * In particular this means that the node is __not__ baking blocks.
/// </summary>
public sealed record ConsensusPassive : INodeConsensusStatus;


/// <summary>
/// The node has been configured with baker keys however it is not currently
/// baking and possilby never will.
/// </summary>
public sealed record NotInCommittee(BakerId BakerId) : INodeConsensusStatus;

/// <summary>
/// The baker keys are registered however the baker is not in the committee
/// for the current 'Epoch'.
/// </summary>
public sealed record AddedButNotActiveInCommittee(BakerId BakerId) : INodeConsensusStatus;

/// <summary>
/// The node has been configured with baker keys that does not match the
/// account.
/// </summary>
public sealed record AddedButWrongKeys(BakerId BakerId) : INodeConsensusStatus;

/// <summary>
/// The node is member of the baking committee.
/// </summary>
public sealed record Baker(BakerId BakerId) : INodeConsensusStatus;

/// <summary>
/// The node is member of the baking and finalization committee.
/// </summary>
public sealed record Finalizer(BakerId BakerId) : INodeConsensusStatus;
