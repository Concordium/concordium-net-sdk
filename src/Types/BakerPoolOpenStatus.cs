using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

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

internal static class BakerPoolOpenStatusFactory
{
    internal static BakerPoolOpenStatus Into(this Grpc.V2.OpenStatus status) =>
        status switch
        {
            Grpc.V2.OpenStatus.OpenForAll => BakerPoolOpenStatus.OpenForAll,
            Grpc.V2.OpenStatus.ClosedForNew => BakerPoolOpenStatus.ClosedForNew,
            Grpc.V2.OpenStatus.ClosedForAll => BakerPoolOpenStatus.ClosedForAll,
            _ => throw new MissingEnumException<Grpc.V2.OpenStatus>(status)
        };
}
