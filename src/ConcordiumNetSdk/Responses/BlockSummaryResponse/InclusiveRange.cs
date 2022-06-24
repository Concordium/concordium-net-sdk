namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

//todo: put in types and make a real type with ctor
/// <summary>
/// Gets or initiates the inclusive range.
/// </summary>
public struct InclusiveRange<T>
{
    /// <summary>
    /// Gets or initiates the min.
    /// </summary>
    public T Min { get; init; }

    /// <summary>
    /// Gets or initiates the max.
    /// </summary>
    public T Max { get; init; }
}
