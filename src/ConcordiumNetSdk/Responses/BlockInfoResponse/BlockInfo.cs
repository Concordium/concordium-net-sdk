using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.BlockInfoResponse;

/// <summary>
/// Represents the information about a particular block with various details as response data of <see cref="ConcordiumNodeClient"/>.<see cref="ConcordiumNodeClient.GetBlockInfoAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/edit/main/docs/grpc.md#getblockinfo--blockhash---blockinfo">here</a>.
/// </summary>
public record BlockInfo
{
    /// <summary>
    /// Gets or initiates the hash of the block (base 16 encoded).
    /// </summary>
    public BlockHash BlockHash { get; init; }

    /// <summary>
    /// Gets or initiates the hash of the parent block.
    /// </summary>
    public BlockHash BlockParent { get; init; }

    /// <summary>
    /// Gets or initiates the hash of the last block that was finalized when this block was created.
    /// </summary>
    public BlockHash BlockLastFinalized { get; init; }

    /// <summary>
    /// Gets or initiates the height of this block in the tree from genesis.
    /// </summary>
    public int BlockHeight { get; init; }

    /// <summary>
    /// Gets or initiates the height of the block since the last protocol update.
    /// </summary>
    public int GenesisIndex { get; init; }

    /// <summary>
    /// Gets or initiates the genesis index (i.e., how many protocol updates have taken effect) of the era the block is in.
    /// </summary>
    public int EraBlockHeight { get; init; }

    /// <summary>
    /// Gets or initiates the time at which the block was received (subjective).
    /// </summary>
    public DateTimeOffset BlockReceiveTime { get; init; }

    /// <summary>
    /// Gets or initiates the time at which the block was validated (subjective).
    /// </summary>
    public DateTimeOffset BlockArriveTime { get; init; }

    /// <summary>
    /// Gets or initiates the slot number of this block. Note that the slot numbers reset on protocol updates.
    /// </summary>
    public int BlockSlot { get; init; }

    /// <summary>
    /// Gets or initiates the time at which this block was nominally baked.
    /// </summary>
    public DateTimeOffset BlockSlotTime { get; init; }

    /// <summary>
    /// Gets or initiates the identity (index) of the baker of this block. Will be null exactly for genesis and regenesis blocks.
    /// </summary>
    public int? BlockBaker { get; init; }

    /// <summary>
    /// Gets or initiates the boolean indicating if this block has been finalized yet.
    /// </summary>
    public bool Finalized { get; init; }

    /// <summary>
    /// Gets or initiates the number of transactions in this block.
    /// </summary>
    public int TransactionCount { get; init; }

    /// <summary>
    /// Gets or initiates the amount of NRG used to execute this block.
    /// </summary>
    public int TransactionEnergyCost { get; init; }

    /// <summary>
    /// Gets or initiates the size of the block's transactions.
    /// </summary>
    public int TransactionsSize { get; init; }

    /// <summary>
    /// Gets or initiates the hash of the block's state.
    /// </summary>
    public string BlockStateHash { get; init; }
}
