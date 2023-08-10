using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Metadata about a given block.
/// </summary>
/// <param name="BlockHash">Hash of the block.</param>
/// <param name="BlockParent">Parent block pointer.</param>
/// <param name="BlockLastFinalized">
/// Pointer to the last finalized block. Each block has a pointer to a
/// specific finalized block that existed at the time the block was
/// produced.
/// </param>
/// <param name="BlockHeight">Height of the block from genesis.</param>
/// <param name="GenesisIndex">
/// The genesis index for this block. This counts the number of protocol
/// updates that have preceded this block, and defines the era of the
/// block.
/// </param>
/// <param name="EraBlockHeight">The height of this block relative to the (re)genesis block of its era.</param>
/// <param name="BlockReceiveTime">
/// Time when the block was first received by the node. This can be in
/// principle quite different from the arrive time if, e.g., block execution
/// takes a long time, or the block must wait for the arrival of its parent.
/// </param>
/// <param name="BlockArriveTime">
/// Time when the block was added to the node's tree. This is a subjective
/// (i.e., node specific) value.
/// </param>
/// <param name="BlockSlot">
/// Slot number of the slot the block is in.
/// Present in protocol versions 1-5.
/// </param>
/// <param name="BlockSlotTime">
/// Slot time of the slot the block is in. In contrast to
/// <see cref="BlockArriveTime"/> this is an objective value, all nodes
/// agree on it.
/// </param>
/// <param name="BlockBaker">
/// Identity of the baker of the block. For non-genesis blocks the value is
/// going to always be not null;
/// </param>
/// <param name="Finalized">Whether the block is finalized or not.</param>
/// <param name="TransactionCount">The number of transactions in the block.</param>
/// <param name="TransactionEnergyCost">The total energy consumption of transactions in the block.</param>
/// <param name="TransactionSize">Size of all the transactions in the block in bytes.</param>
/// <param name="BlockStateHash">Hash of the block state at the end of the given block.</param>
/// <param name="ProtocolVersion">Protocol version to which the block belongs.</param>
/// <param name="Round">The round of the block. Present from protocol version 6.</param>
/// <param name="Epoch">The epoch of the block. Present from protocol version 6.</param>
public sealed record BlockInfo(
    BlockHash BlockHash,
    BlockHash BlockParent,
    BlockHash BlockLastFinalized,
    ulong BlockHeight,
    uint GenesisIndex,
    ulong EraBlockHeight,
    DateTimeOffset BlockReceiveTime,
    DateTimeOffset BlockArriveTime,
    ulong? BlockSlot,
    DateTimeOffset BlockSlotTime,
    BakerId? BlockBaker,
    bool Finalized,
    uint TransactionCount,
    EnergyAmount TransactionEnergyCost,
    uint TransactionSize,
    StateHash BlockStateHash,
    ProtocolVersion ProtocolVersion,
    Round? Round,
    Epoch? Epoch
    )
{
    internal static BlockInfo From(Grpc.V2.BlockInfo blockInfo) =>
        new(
            BlockHash: BlockHash.From(blockInfo.Hash),
            BlockParent: BlockHash.From(blockInfo.ParentBlock),
            BlockLastFinalized: BlockHash.From(blockInfo.LastFinalizedBlock),
            BlockHeight: blockInfo.Height.Value,
            GenesisIndex: blockInfo.GenesisIndex.Value,
            EraBlockHeight: blockInfo.EraBlockHeight.Value,
            BlockReceiveTime: blockInfo.ReceiveTime.ToDateTimeOffset(),
            BlockArriveTime: blockInfo.ArriveTime.ToDateTimeOffset(),
            BlockSlot: blockInfo.SlotNumber?.Value,
            BlockSlotTime: blockInfo.SlotTime.ToDateTimeOffset(),
            BlockBaker: blockInfo.Baker != null ? BakerId.From(blockInfo.Baker) : null,
            Finalized: blockInfo.Finalized,
            TransactionCount: blockInfo.TransactionCount,
            TransactionEnergyCost: new EnergyAmount(blockInfo.TransactionsEnergyCost.Value),
            TransactionSize: blockInfo.TransactionsSize,
            BlockStateHash: new StateHash(blockInfo.StateHash.Value),
            ProtocolVersion: blockInfo.ProtocolVersion.Into(),
            Round: blockInfo.Round != null ? Types.Round.From(blockInfo.Round) : null,
            Epoch: blockInfo.Epoch != null ? Types.Epoch.From(blockInfo.Epoch) : null
        );
}
