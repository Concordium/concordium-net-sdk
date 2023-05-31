namespace Concordium.Sdk.Types.New;

public class FinalizationData
{
    public BlockHash FinalizationBlockPointer { get; init; }
    public long FinalizationIndex { get; init; } // Haskell datatype: FinalizationIndex : Word64
    public long FinalizationDelay { get; init; } // Haskell datatype: BlockHeight : Word64
    public FinalizationSummaryParty[] Finalizers { get; init; }
}