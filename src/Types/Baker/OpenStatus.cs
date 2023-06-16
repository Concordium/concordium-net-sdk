using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types.Baker;

/// <summary>
/// The status of whether a baking pool allows delegators to join.
/// </summary>
public enum OpenStatus
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
    ClosedForAll
}

internal static class OpenStatusFactory
{
    internal static OpenStatus Into(this Grpc.V2.OpenStatus status) =>
        status switch
        {
            Grpc.V2.OpenStatus.OpenForAll => OpenStatus.OpenForAll,
            Grpc.V2.OpenStatus.ClosedForNew => OpenStatus.ClosedForNew,
            Grpc.V2.OpenStatus.ClosedForAll => OpenStatus.ClosedForAll,
            _ => throw new MissingEnumException<Grpc.V2.OpenStatus>(status)
        };
}
