namespace Concordium.Sdk.Types.New;

public enum BakerPoolOpenStatus 
{
    /// <summary>
    /// New delegators may join the pool.
    /// </summary>
    OpenForAll,
    /// <summary>
    /// New delegators may not join, but existing delegators are kept.
    /// </summary>
    ClosedForNew,
    /// <summary>
    /// No delegators are allowed. 
    /// </summary>
    ClosedForAll,
}