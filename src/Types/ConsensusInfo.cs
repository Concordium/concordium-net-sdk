using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Summary of the current state of consensus.
/// </summary>
/// <param name="BestBlock">
/// Hash of the current best block. The best block is a protocol defined
/// block that the node must use as parent block to build the chain on.
/// Note that this is subjective, in the sense that it is only the best
/// block among the blocks the node knows about.
/// </param>
/// <param name="GenesisBlock">Hash of the genesis block.</param>
/// <param name="GenesisTime">Slot time of the genesis block.</param>
/// <param name="LastFinalizedBlock">Hash of the last, i.e., most recent, finalized block.</param>
/// <param name="BestBlockHeight">Height of the best block. See <see cref="BestBlock"/>.</param>
/// <param name="LastFinalizedBlockHeight">Height of the last finalized block. Genesis block has height 0.</param>
/// <param name="ProtocolVersion">Currently active protocol version.</param>
/// <param name="CurrentEraGenesisBlock">
/// Block hash of the genesis block of current era, i.e., since the last
/// protocol update. Initially this is equal to
/// <see cref="GenesisBlock"/>.
/// </param>
/// <param name="CurrentEraGenesisTime">Time when the current era started.</param>
/// <param name="SlotDuration">Duration of a slot.</param>
/// <param name="EpochDuration">Duration of an epoch.</param>
/// <param name="GenesisIndex">
/// The number of chain restarts via a protocol update. An effected
/// protocol update instruction might not change the protocol version
/// specified in <see cref="ProtocolVersion"/>, but it always increments the genesis
/// index.
/// </param>
/// <param name="BlocksReceivedCount">The number of blocks that have been received.</param>
/// <param name="BlockLastReceivedTime">The time (local time of the node) that a block was last received.</param>
/// <param name="BlockReceiveLatencyEma">
/// Exponential moving average of block receive latency (in seconds), i.e.
/// the time between a block's nominal slot time, and the time at which is
/// received.
/// </param>
/// <param name="BlockReceiveLatencyEmsd">
/// Exponential moving average standard deviation of block receive latency
/// (in seconds), i.e. the time between a block's nominal slot time, and
/// the time at which is received.
/// </param>
/// <param name="BlockReceivePeriodEma">
/// Exponential moving average of the time between receiving blocks (in
/// seconds).
/// </param>
/// <param name="BlockReceivePeriodEmsd">
/// Exponential moving average standard deviation of the time between
/// receiving blocks (in seconds).
/// </param>
/// <param name="BlocksVerifiedCount">
/// Number of blocks that arrived, i.e., were added to the tree. Note that
/// in some cases this can be more than <see cref="BlocksReceivedCount"/>
/// since blocks that the node itself produces count towards this,
/// but are not received.
/// </param>
/// <param name="BlockLastArrivedTime">
/// The time (local time of the node) that a block last arrived, i.e., was
/// verified and added to the node's tree.
/// </param>
/// <param name="BlockArriveLatencyEma">
/// The exponential moving average of the time between a block's nominal
/// slot time, and the time at which it is verified.
/// </param>
/// <param name="BlockArriveLatencyEmsd">
/// The exponential moving average standard deviation of the time between a
/// block's nominal slot time, and the time at which it is verified.
/// </param>
/// <param name="BlockArrivePeriodEma">
/// Exponential moving average of the time between receiving blocks (in
/// seconds).
/// </param>
/// <param name="BlockArrivePeriodEmsd">
/// Exponential moving average standard deviation of the time between blocks
/// being verified.
/// </param>
/// <param name="TransactionsPerBlockEma">
/// Exponential moving average of the number of
/// transactions per block.
/// </param>
/// <param name="TransactionsPerBlockEmsd">
/// Exponential moving average standard deviation of the number of
/// transactions per block.
/// </param>
/// <param name="FinalizationCount">The number of completed finalizations.</param>
/// <param name="LastFinalizedTime">
/// Time at which a block last became finalized. Note that this is the local
/// time of the node at the time the block was finalized.
/// </param>
/// <param name="FinalizationPeriodEma">
/// Exponential moving average of the time between finalizations. Will be
/// null if there are no finalizations yet since the node start.
/// </param>
/// <param name="FinalizationPeriodEmsd">
/// Exponential moving average standard deviation of the time between
/// finalizations. Will be `None` if there are no finalizations yet
/// since the node start.
/// </param>
public sealed record ConsensusInfo(
    BlockHash BestBlock,
    BlockHash GenesisBlock,
    DateTimeOffset GenesisTime,
    BlockHash LastFinalizedBlock,
    ulong BestBlockHeight,
    ulong LastFinalizedBlockHeight,
    ProtocolVersion ProtocolVersion,
    BlockHash CurrentEraGenesisBlock,
    DateTimeOffset CurrentEraGenesisTime,
    TimeSpan SlotDuration,
    TimeSpan EpochDuration,
    uint GenesisIndex,
    uint BlocksReceivedCount,
    DateTimeOffset? BlockLastReceivedTime,
    double BlockReceiveLatencyEma,
    double BlockReceiveLatencyEmsd,
    double? BlockReceivePeriodEma,
    double? BlockReceivePeriodEmsd,
    uint BlocksVerifiedCount,
    DateTimeOffset? BlockLastArrivedTime,
    double BlockArriveLatencyEma,
    double BlockArriveLatencyEmsd,
    double? BlockArrivePeriodEma,
    double? BlockArrivePeriodEmsd,
    double TransactionsPerBlockEma,
    double TransactionsPerBlockEmsd,
    ulong FinalizationCount,
    DateTimeOffset? LastFinalizedTime,
    double? FinalizationPeriodEma,
    double? FinalizationPeriodEmsd
    )
{
    internal static ConsensusInfo From(Grpc.V2.ConsensusInfo consensusInfo) =>
        new
        (
            BestBlock: BlockHash.From(consensusInfo.BestBlock),
            GenesisBlock: BlockHash.From(consensusInfo.GenesisBlock),
            GenesisTime: consensusInfo.GenesisTime.ToDateTimeOffset(),
            LastFinalizedBlock: BlockHash.From(consensusInfo.LastFinalizedBlock),
            BestBlockHeight: consensusInfo.BestBlockHeight.Value,
            LastFinalizedBlockHeight: consensusInfo.LastFinalizedBlockHeight.Value,
            ProtocolVersion: consensusInfo.ProtocolVersion.Into(),
            CurrentEraGenesisBlock: BlockHash.From(consensusInfo.CurrentEraGenesisBlock),
            CurrentEraGenesisTime: consensusInfo.CurrentEraGenesisTime.ToDateTimeOffset(),
            SlotDuration: TimeSpan.FromMilliseconds(consensusInfo.SlotDuration.Value),
            EpochDuration: TimeSpan.FromMilliseconds(consensusInfo.EpochDuration.Value),
            GenesisIndex: consensusInfo.GenesisIndex.Value,
            BlocksReceivedCount: consensusInfo.BlocksReceivedCount,
            BlockLastReceivedTime: consensusInfo.BlockLastReceivedTime?.ToDateTimeOffset(),
            BlockReceiveLatencyEma: consensusInfo.BlockReceiveLatencyEma,
            BlockReceiveLatencyEmsd: consensusInfo.BlockReceiveLatencyEmsd,
            BlockReceivePeriodEma: consensusInfo.HasBlockReceivePeriodEma ? consensusInfo.BlockReceivePeriodEma : null,
            BlockReceivePeriodEmsd: consensusInfo.HasBlockReceivePeriodEmsd ? consensusInfo.BlockReceivePeriodEmsd : null,
            BlocksVerifiedCount: consensusInfo.BlocksVerifiedCount,
            BlockLastArrivedTime: consensusInfo.BlockLastReceivedTime?.ToDateTimeOffset(),
            BlockArriveLatencyEma: consensusInfo.BlockArriveLatencyEma,
            BlockArriveLatencyEmsd: consensusInfo.BlockArriveLatencyEmsd,
            BlockArrivePeriodEma: consensusInfo.HasBlockArrivePeriodEma ? consensusInfo.BlockArrivePeriodEma : null,
            BlockArrivePeriodEmsd: consensusInfo.HasBlockArrivePeriodEmsd ? consensusInfo.BlockArrivePeriodEmsd : null,
            TransactionsPerBlockEma: consensusInfo.TransactionsPerBlockEma,
            TransactionsPerBlockEmsd: consensusInfo.TransactionsPerBlockEmsd,
            FinalizationCount: consensusInfo.FinalizationCount,
            LastFinalizedTime: consensusInfo.LastFinalizedTime?.ToDateTimeOffset(),
            FinalizationPeriodEma: consensusInfo.HasFinalizationPeriodEma ? consensusInfo.FinalizationPeriodEma : null,
            FinalizationPeriodEmsd: consensusInfo.HasFinalizationPeriodEmsd ? consensusInfo.FinalizationPeriodEmsd : null
        );
}
