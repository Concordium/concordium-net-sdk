using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Summary of the finalization record in a block, if any.
/// </summary>
/// <param name="BlockPointer">Block that was finalized by the finalization record.</param>
/// <param name="Index">Index of the finalization round that finalized the block.</param>
/// <param name="Delay">Finalization delay used for the finalization round.</param>
/// <param name="Finalizers">
/// List of all finalizers with information about whether they signed the
/// finalization record or not.
/// </param>
public sealed record FinalizationSummary(
    BlockHash BlockPointer,
    ulong Index,
    ulong Delay,
    FinalizationSummaryParty[] Finalizers
)
{
    internal static FinalizationSummary? From(BlockFinalizationSummary summary) =>
        summary.SummaryCase switch
        {
            BlockFinalizationSummary.SummaryOneofCase.Record =>
                new FinalizationSummary(
                    BlockHash.From(summary.Record.Block),
                    summary.Record.Index.Value,
                    summary.Record.Delay.Value,
                    summary.Record.Finalizers
                        .Select(FinalizationSummaryParty.From)
                        .ToArray()),
            BlockFinalizationSummary.SummaryOneofCase.None_ =>
                null,
            BlockFinalizationSummary.SummaryOneofCase.None =>
                throw new MissingEnumException<BlockFinalizationSummary.SummaryOneofCase>(summary.SummaryCase),
            _ => throw new MissingEnumException<BlockFinalizationSummary.SummaryOneofCase>(summary.SummaryCase)
        };
}
