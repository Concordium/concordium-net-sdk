using System.Text.Json.Serialization;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.ConsensusStatusResponse;

/// <summary>
/// Represents the information about a current state of the consensus layer as response data of <see cref="ConcordiumNodeClient"/>.<see cref="ConcordiumNodeClient.GetConsensusStatusAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/blob/main/docs/grpc.md#getconsensusstatus--consensusstatus">here</a>.
/// These fields are broken down into four categories.
/// </summary>
public record ConsensusStatus
{
    // General
    // These fields are updated as they change, which is typically as a result of a block arriving (being validated)
    // or a finalization record being validated.
    
    /// <summary>
    /// Gets or initiates the hash of the current best block in the tree.
    /// </summary>
    public BlockHash BestBlock { get; init; }
    
    /// <summary>
    /// Gets or initiates the hash of the genesis block (never changes).
    /// </summary>
    public BlockHash GenesisBlock { get; init; }
    
    /// <summary>
    /// Gets or initiates the genesis time as specified in the genesis block (never changes for a given network).
    /// </summary>
    public DateTimeOffset GenesisTime { get; init; }
    
    /// <summary>
    /// Gets or initiates the hash of the last finalized block.
    /// </summary>
    public BlockHash LastFinalizedBlock { get; init; }
    
    /// <summary>
    /// Gets or initiates the height of the best block.
    /// </summary>
    public int BestBlockHeight { get; init; }
    
    /// <summary>
    /// Gets or initiates the height of the last finalized block.
    /// </summary>
    public int LastFinalizedBlockHeight { get; init; }
    
    /// <summary>
    /// Gets or initiates the the currently active protocol version. This determines supported transactions,
    /// their behaviour, as well as general behaviour of consensus. Currently protocol versions 1 and 2 are possible.
    /// </summary>
    public int ProtocolVersion { get; init; }
    
    /// <summary>
    /// Gets or initiates the hash of the genesis block after the latest protocol update.
    /// </summary>
    public BlockHash CurrentEraGenesisBlock { get; init; }
    
    /// <summary>
    /// Gets or initiates the slot time of the genesis block for the current era.
    /// </summary>
    public DateTimeOffset CurrentEraGenesisTime { get; init; }
    
    /// <summary>
    /// Gets or initiates the duration in milliseconds of a slot. This only changes on protocol updates.
    /// </summary>
    public int SlotDuration { get; init; }
    
    /// <summary>
    /// Gets or initiates the duration in milliseconds of an epoch. This only changes on protocol updates.
    /// </summary>
    public int EpochDuration { get; init; }
    
    /// <summary>
    /// Gets or initiates the number of protocol updates that have taken effect.
    /// </summary>
    public int GenesisIndex { get; init; }

    // Received block statistics
    // These statistics are updated whenever a block is received from the network. A received block is not immediately
    // added to the tree; first it must be validated, which takes some time, and may require data (such as other blocks)
    // that are not available at the time it is received. When graphing these statistics against time, the time point
    // should be taken as blockLastReceivedTime.
    // Exponential moving averages are computed with weight 0.1 (that is, the latest value is weighted 1:9 with
    // the previous average). When rendering these EMAs as a graph, the standard deviations can be rendered as
    // error bars; they give an idea of how variable the statistic is.
    
    /// <summary>
    /// Gets or initiates the number of blocks that have been received from peers.
    /// </summary>
    public int BlocksReceivedCount { get; init; }
    
    /// <summary>
    /// Gets or initiates the time that a block was last received 
    /// </summary>
    public DateTimeOffset? BlockLastReceivedTime { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of block receive latency (in seconds), i.e.
    /// the time between a block's nominal slot time, and the time at which is received.
    /// </summary>
    [JsonPropertyName("blockReceiveLatencyEMA")]
    public double BlockReceiveLatencyEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of block receive latency.
    /// </summary>
    [JsonPropertyName("blockReceiveLatencyEMSD")]
    public double BlockReceiveLatencyEmsd { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of the time between receiving blocks (in seconds).
    /// </summary>
    [JsonPropertyName("blockReceivePeriodEMA")]
    public double? BlockReceivePeriodEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of time between receiving blocks.
    /// </summary>
    [JsonPropertyName("blockReceivePeriodEMSD")]
    public double? BlockReceivePeriodEmsd { get; init; }
        
    // Verified block statistics
    // These statistics are updated whenever a block arrives (that is, verified and added to the block tree).
    // When graphing these statistics against time, the time point should be taken as blockLastArrivedTime.
    
    /// <summary>
    /// Gets or initiates the number of blocks that have arrived. Note that blocks produced by the node itself are
    /// counted in this statistic, but not in blocksReceivedCount.
    /// </summary>
    public int BlocksVerifiedCount { get; init; }
    
    /// <summary>
    /// Gets or initiates the time that a block last arrived.
    /// </summary>
    public DateTimeOffset? BlockLastArrivedTime { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of the time between a block's nominal
    /// slot time, and the time at which it is verified.
    /// </summary>
    [JsonPropertyName("blockArriveLatencyEMA")]
    public double BlockArriveLatencyEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of the time between a block's nominal
    /// slot time, and the time at which it is verified.
    /// </summary>
    [JsonPropertyName("blockArriveLatencyEMSD")]
    public double BlockArriveLatencyEmsd { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of the time between blocks being verified.
    /// </summary>
    [JsonPropertyName("blockArrivePeriodEMA")]
    public double? BlockArrivePeriodEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of the time between blocks being verified.
    /// </summary>
    [JsonPropertyName("blockArrivePeriodEMSD")]
    public double? BlockArrivePeriodEmsd { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of number of transactions per block.
    /// </summary>
    [JsonPropertyName("transactionsPerBlockEMA")]
    public double TransactionsPerBlockEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of number of transactions per block.
    /// </summary>
    [JsonPropertyName("transactionsPerBlockEMSD")]
    public double TransactionsPerBlockEmsd { get; init; }

    // Finalization statistics
    // These statistics are updated whenever a block is finalized. When graphing these statistics against time,
    // the time point should be taken as lastFinalizedTime.
    
    /// <summary>
    /// Gets or initiates the number of finalization records that have been validated; this will be less than the
    /// lastFinalizedBlockHeight, since a finalization record can finalize multiple blocks. 
    /// </summary>
    public int FinalizationCount { get; init; }
    
    /// <summary>
    /// Gets or initiates the time at which a block last became finalized (will be null until the node observes a finalization).
    /// </summary>
    public DateTimeOffset? LastFinalizedTime { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving average of the time between finalizations (will be null until a node observes finalizations).
    /// </summary>
    [JsonPropertyName("finalizationPeriodEMA")]
    public double? FinalizationPeriodEma { get; init; }
    
    /// <summary>
    /// Gets or initiates the exponential moving standard deviation of the time between finalizations (will be null until a node observes finalizations).
    /// </summary>
    [JsonPropertyName("finalizationPeriodEMSD")]
    public double? FinalizationPeriodEmsd { get; init; }
}
