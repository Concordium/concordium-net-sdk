using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Metadata about a given block.
/// </summary>
public class BlockInfo
{
    /// <summary>
    /// Hash of the block.
    /// </summary>
    public BlockHash BlockHash { get; init; }
    /// <summary>
    /// Parent block pointer.
    /// </summary>
    public BlockHash BlockParent { get; init; }
    /// <summary>
    /// Pointer to the last finalized block. Each block has a pointer to a
    /// specific finalized block that existed at the time the block was
    /// produced.
    /// </summary>
    public BlockHash BlockLastFinalized { get; init; }
    /// <summary>
    /// Height of the block from genesis.
    /// </summary>
    public ulong BlockHeight { get; init; }
    /// <summary>
    /// The genesis index for this block. This counts the number of protocol
    /// updates that have preceded this block, and defines the era of the
    /// block.
    /// </summary>
    public uint GenesisIndex { get; init; }
    /// <summary>
    /// The height of this block relative to the (re)genesis block of its era.
    /// </summary>
    public ulong EraBlockHeight { get; init; }
    /// <summary>
    /// Time when the block was first received by the node. This can be in
    /// principle quite different from the arrive time if, e.g., block execution
    /// takes a long time, or the block must wait for the arrival of its parent.
    /// </summary>
    public DateTimeOffset BlockReceiveTime { get; init; }
    /// <summary>
    /// Time when the block was added to the node's tree. This is a subjective
    /// (i.e., node specific) value.
    /// </summary>
    public DateTimeOffset BlockArriveTime { get; init; }
    /// <summary>
    /// Slot number of the slot the block is in.
    /// </summary>
    public ulong BlockSlot { get; init; }
    /// <summary>
    /// Slot time of the slot the block is in. In contrast to
    /// <see cref="BlockArriveTime"/> this is an objective value, all nodes
    /// agree on it.
    /// </summary>
    public DateTimeOffset BlockSlotTime { get; init; }
    /// <summary>
    /// Identity of the baker of the block. For non-genesis blocks the value is
    /// going to always be not null;
    /// </summary>
    public ulong? BlockBaker { get; init; }
    /// <summary>
    /// Whether the block is finalized or not.
    /// </summary>
    public bool Finalized { get; init; }
    /// <summary>
    /// The number of transactions in the block.
    /// </summary>
    public uint TransactionCount { get; init; }
    /// <summary>
    /// The total energy consumption of transactions in the block.
    /// </summary>
    public EnergyAmount TransactionEnergyCost { get; init; }
    /// <summary>
    /// Size of all the transactions in the block in bytes.
    /// </summary>
    public uint TransactionSize { get; init; }
    /// <summary>
    /// Hash of the block state at the end of the given block.
    /// </summary>
    public StateHash BlockStateHash { get; init; }
    /// <summary>
    /// Protocol version to which the block belongs.
    /// </summary>
    public ProtocolVersion ProtocolVersion { get; init; }

    internal static BlockInfo From(Grpc.V2.BlockInfo blockInfo) =>
        new()
        {
            BlockHash = BlockHash.From(blockInfo.Hash),
            BlockParent = BlockHash.From(blockInfo.ParentBlock),
            BlockLastFinalized = BlockHash.From(blockInfo.LastFinalizedBlock),
            BlockHeight = blockInfo.Height.Value,
            GenesisIndex = blockInfo.GenesisIndex.Value,
            EraBlockHeight = blockInfo.EraBlockHeight.Value,
            BlockReceiveTime = blockInfo.ReceiveTime.ToDateTimeOffset(),
            BlockArriveTime = blockInfo.ArriveTime.ToDateTimeOffset(),
            BlockSlot = blockInfo.SlotNumber.Value,
            BlockSlotTime = blockInfo.SlotTime.ToDateTimeOffset(),
            BlockBaker = blockInfo.Baker?.Value,
            Finalized = blockInfo.Finalized,
            TransactionCount = blockInfo.TransactionsSize,
            TransactionEnergyCost = new EnergyAmount(blockInfo.TransactionsEnergyCost.Value),
            TransactionSize = blockInfo.TransactionsSize,
            BlockStateHash = new StateHash(blockInfo.StateHash.Value),
            ProtocolVersion = blockInfo.ProtocolVersion.Into()
        };
}
