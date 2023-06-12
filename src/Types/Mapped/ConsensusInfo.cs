using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Summary of the current state of consensus.
/// </summary>
public class ConsensusInfo
{
    // General
    /// <summary>
    /// Hash of the current best block. The best block is a protocol defined
    /// block that the node must use as parent block to build the chain on.
    /// Note that this is subjective, in the sense that it is only the best
    /// block among the blocks the node knows about.
    /// </summary>
    public BlockHash BestBlock { get; init; }
    /// <summary>
    /// Hash of the genesis block.
    /// </summary>
    public BlockHash GenesisBlock { get; init; }
    /// <summary>
    /// Slot time of the genesis block.
    /// </summary>
    public DateTimeOffset GenesisTime { get; init; }
    /// <summary>
    /// Hash of the last, i.e., most recent, finalized block.
    /// </summary>
    public BlockHash LastFinalizedBlock { get; init; }
    /// <summary>
    /// Height of the best block. See <see cref="BestBlock"/>.
    /// </summary>
    public ulong BestBlockHeight { get; init; }
    /// <summary>
    /// Height of the last finalized block. Genesis block has height 0.
    /// </summary>
    public ulong LastFinalizedBlockHeight { get; init; }
    /// <summary>
    /// Currently active protocol version.
    /// </summary>
    public ProtocolVersion ProtocolVersion { get; init; }
    /// <summary>
    /// Block hash of the genesis block of current era, i.e., since the last
    /// protocol update. Initially this is equal to
    /// <see cref="GenesisBlock"/>.
    /// </summary>
    public BlockHash CurrentEraGenesisBlock { get; init; }
    /// <summary>
    /// Time when the current era started.
    /// </summary>
    public DateTimeOffset CurrentEraGenesisTime { get; init; }
    /// <summary>
    /// Duration of a slot  .
    /// </summary>
    public TimeSpan SlotDuration { get; init; }
    /// <summary>
    /// Duration of an epoch.
    /// </summary>
    public TimeSpan EpochDuration { get; init; }
    /// <summary>
    /// The number of chain restarts via a protocol update. An effected
    /// protocol update instruction might not change the protocol version
    /// specified in <see cref="ProtocolVersion"/>, but it always increments the genesis
    /// index.
    /// </summary>
    public uint GenesisIndex { get; init; }

    // Received block statistics
    /// <summary>
    /// The number of blocks that have been received.
    /// </summary>
    public uint BlocksReceivedCount { get; init; }
    /// <summary>
    /// The time (local time of the node) that a block was last received.
    /// </summary>
    public DateTimeOffset? BlockLastReceivedTime { get; init; }
    /// <summary>
    /// Exponential moving average of block receive latency (in seconds), i.e.
    /// the time between a block's nominal slot time, and the time at which is
    /// received.
    /// </summary>
    public double BlockReceiveLatencyEma { get; init; }
    /// <summary>
    /// Exponential moving average standard deviation of block receive latency
    /// (in seconds), i.e. the time between a block's nominal slot time, and
    /// the time at which is received.
    /// </summary>
    public double BlockReceiveLatencyEmsd { get; init; }
    /// <summary>
    /// Exponential moving average of the time between receiving blocks (in
    /// seconds).
    /// </summary>
    public double? BlockReceivePeriodEma { get; init; }
    /// <summary>
    /// Exponential moving average standard deviation of the time between
    /// receiving blocks (in seconds).
    /// </summary>
    public double? BlockReceivePeriodEmsd { get; init; }

    // Verified block statistics
    /// <summary>
    /// Number of blocks that arrived, i.e., were added to the tree. Note that
    /// in some cases this can be more than <see cref="BlocksReceivedCount"/>
    /// since blocks that the node itself produces count towards this,
    /// but are not received.
    /// </summary>
    public uint BlocksVerifiedCount { get; init; }
    /// <summary>
    /// The time (local time of the node) that a block last arrived, i.e., was
    /// verified and added to the node's tree.
    /// </summary>
    public DateTimeOffset? BlockLastArrivedTime { get; init; }
    /// <summary>
    /// The exponential moving average of the time between a block's nominal
    /// slot time, and the time at which it is verified.
    /// </summary>
    public double BlockArriveLatencyEma { get; init; }
    /// <summary>
    /// The exponential moving average standard deviation of the time between a
    /// block's nominal slot time, and the time at which it is verified.
    /// </summary>
    public double BlockArriveLatencyEmsd { get; init; }
    /// <summary>
    /// Exponential moving average of the time between receiving blocks (in
    /// seconds).
    /// </summary>
    public double? BlockArrivePeriodEma { get; init; }
    /// <summary>
    /// Exponential moving average standard deviation of the time between blocks
    /// being verified.
    /// </summary>
    public double? BlockArrivePeriodEmsd { get; init; }
    /// <summary>
    /// Exponential moving average of the number of
    /// transactions per block.
    /// </summary>
    public double TransactionsPerBlockEma { get; init; }
    /// <summary>
    /// Exponential moving average standard deviation of the number of
    /// transactions per block.
    /// </summary>
    public double TransactionsPerBlockEmsd { get; init; }

    // Finalization statistics
    /// <summary>
    /// The number of completed finalizations.
    /// </summary>
    public ulong FinalizationCount { get; init; }
    /// <summary>
    /// Time at which a block last became finalized. Note that this is the local
    /// time of the node at the time the block was finalized.
    /// </summary>
    public DateTimeOffset? LastFinalizedTime { get; init; }
    /// <summary>
    /// Exponential moving average of the time between finalizations. Will be
    /// `None` if there are no finalizations yet since the node start.
    /// </summary>
    public double? FinalizationPeriodEma { get; init; }
    /// <summary>
    /// Exponential moving average standard deviation of the time between
    /// finalizations. Will be `None` if there are no finalizations yet
    /// since the node start.
    /// </summary>
    public double? FinalizationPeriodEmsd { get; init; }

    internal static ConsensusInfo From(Grpc.V2.ConsensusInfo consensusInfo) =>
        new()
        {
            BestBlock = BlockHash.From(consensusInfo.BestBlock),
            GenesisBlock = BlockHash.From(consensusInfo.GenesisBlock),
            GenesisTime = consensusInfo.GenesisTime.ToDateTimeOffset(),
            LastFinalizedBlock = BlockHash.From(consensusInfo.LastFinalizedBlock),
            BestBlockHeight = consensusInfo.BestBlockHeight.Value,
            LastFinalizedBlockHeight = consensusInfo.LastFinalizedBlockHeight.Value,
            ProtocolVersion = consensusInfo.ProtocolVersion.Into(),
            CurrentEraGenesisBlock = BlockHash.From(consensusInfo.CurrentEraGenesisBlock),
            CurrentEraGenesisTime = consensusInfo.CurrentEraGenesisTime.ToDateTimeOffset(),
            SlotDuration = TimeSpan.FromMilliseconds(consensusInfo.SlotDuration.Value),
            EpochDuration = TimeSpan.FromMilliseconds(consensusInfo.EpochDuration.Value),
            GenesisIndex = consensusInfo.GenesisIndex.Value,
            BlocksReceivedCount = consensusInfo.BlocksReceivedCount,
            BlockLastReceivedTime = consensusInfo.BlockLastReceivedTime?.ToDateTimeOffset(),
            BlockReceiveLatencyEma = consensusInfo.BlockReceiveLatencyEma,
            BlockReceiveLatencyEmsd = consensusInfo.BlockReceiveLatencyEmsd,
            BlockReceivePeriodEma = consensusInfo.HasBlockReceivePeriodEma ? consensusInfo.BlockReceivePeriodEma : null,
            BlockReceivePeriodEmsd = consensusInfo.HasBlockReceivePeriodEmsd ? consensusInfo.BlockReceivePeriodEmsd : null,
            BlocksVerifiedCount = consensusInfo.BlocksVerifiedCount,
            BlockLastArrivedTime = consensusInfo.BlockLastReceivedTime?.ToDateTimeOffset(),
            BlockArriveLatencyEma = consensusInfo.BlockArriveLatencyEma,
            BlockArriveLatencyEmsd = consensusInfo.BlockArriveLatencyEmsd,
            BlockArrivePeriodEma = consensusInfo.HasBlockArrivePeriodEma ? consensusInfo.BlockArrivePeriodEma : null,
            BlockArrivePeriodEmsd = consensusInfo.HasBlockArrivePeriodEmsd ? consensusInfo.BlockArrivePeriodEmsd : null,
            TransactionsPerBlockEma = consensusInfo.TransactionsPerBlockEma,
            TransactionsPerBlockEmsd = consensusInfo.TransactionsPerBlockEmsd,
            FinalizationCount = consensusInfo.FinalizationCount,
            LastFinalizedTime = consensusInfo.LastFinalizedTime?.ToDateTimeOffset(),
            FinalizationPeriodEma = consensusInfo.HasFinalizationPeriodEma ? consensusInfo.FinalizationPeriodEma : null,
            FinalizationPeriodEmsd = consensusInfo.HasFinalizationPeriodEmsd ? consensusInfo.FinalizationPeriodEmsd : null,
        };
}
