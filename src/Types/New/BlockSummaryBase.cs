using Concordium.Grpc.V2;
using Concordium.Sdk.Types.Mapped;

namespace Concordium.Sdk.Types.New;

public abstract class BlockSummaryBase
{
    public int? ProtocolVersion { get; init; }
    public TransactionSummary[] TransactionSummaries { get; init; }
    public ISpecialEvent[] SpecialEvents { get; init; }
    public FinalizationData? FinalizationData { get; init; }

    public IEnumerable<AccountBalanceUpdate> GetAccountBalanceUpdates()
    {
        foreach (var item in this.TransactionSummaries.SelectMany(x => x.GetAccountBalanceUpdates()))
            yield return item;

        foreach (var item in this.SpecialEvents.SelectMany(x => x.GetAccountBalanceUpdates()))
            yield return item;
    }

    public static BlockSummaryBase From(IAsyncEnumerable<BlockSpecialEvent> events,
        BlockFinalizationSummary finalization,
        IAsyncEnumerable<Concordium.Grpc.V2.BlockItemSummary> blockTransactionEvents)
    {
        // TODO: missing information to map Protocol Version
        throw new NotImplementedException();
    }
}
