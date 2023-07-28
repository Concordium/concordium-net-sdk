namespace Concordium.Sdk.Types;

/// <summary>
/// Round number. Applies to protocol 6 and onward.
/// </summary>
/// <param name="RoundNumber"></param>
public readonly record struct Round(ulong RoundNumber)
{
    internal static Round From(Grpc.V2.Round round) => new(round.Value);
}
