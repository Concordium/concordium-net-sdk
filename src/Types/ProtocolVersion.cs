using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// An enumeration of the supported versions of the consensus protocol.
/// </summary>
public enum ProtocolVersion
{
    /// <summary>
    /// The initial protocol version at mainnet launch.
    /// </summary>
    P1 = 1,
    /// <summary>
    /// Protocol `P2` introduces support for transfers with memos.
    /// </summary>
    P2,
    /// <summary>
    /// Protocol `P3` introduces support for account aliases. Each account can
    /// now be referred to by `2^24` different addresses.
    /// </summary>
    P3,
    /// <summary>
    /// Protocol `P4` is a major upgrade that adds support for delegation,
    /// baking pools, and V1 smart contracts.
    /// </summary>
    P4,
    /// <summary>
    /// Protocol `P5` is a minor upgrade that adds support for smart contract
    /// upgradability, smart contract queries, relaxes some limitations and
    /// improves the structure of internal node datastructures related to
    /// accounts.
    /// </summary>
    P5,
    /// Protocol `P6` is a future protocol update that will introduce a new
    /// consensus.
    P6,
    /// <summary>
    /// Protocol `P7` modifies hashing to better support light clients, and
    /// implements tokenomics changes.
    /// </summary>
    P7,
    /// <summary>
    /// Protocol `P8` supports suspending inactive validators automatically
    /// when they consistently fail to participate in the consensus protocol.
    /// Suspended validators can be reactivated manually by a transaction
    /// from the validator's account.
    /// </summary>
    P8,
}


/// <summary>
/// Extension methods for Protocol Version
/// </summary>
public static class ProtocolVersionExtension
{
    /// <summary>
    /// Parse to integer.
    /// </summary>>
    public static int AsInt(this ProtocolVersion protocolVersion) => (int)protocolVersion;
}

internal static class ProtocolVersionFactory
{
    internal static ProtocolVersion Into(this Grpc.V2.ProtocolVersion protocolVersion) =>
        protocolVersion switch
        {
            Grpc.V2.ProtocolVersion._1 => ProtocolVersion.P1,
            Grpc.V2.ProtocolVersion._2 => ProtocolVersion.P2,
            Grpc.V2.ProtocolVersion._3 => ProtocolVersion.P3,
            Grpc.V2.ProtocolVersion._4 => ProtocolVersion.P4,
            Grpc.V2.ProtocolVersion._5 => ProtocolVersion.P5,
            Grpc.V2.ProtocolVersion._6 => ProtocolVersion.P6,
            Grpc.V2.ProtocolVersion._7 => ProtocolVersion.P7,
            Grpc.V2.ProtocolVersion._8 => ProtocolVersion.P8,
            _ => throw new MissingEnumException<Grpc.V2.ProtocolVersion>(protocolVersion)
        };
}
