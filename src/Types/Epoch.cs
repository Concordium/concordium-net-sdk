namespace Concordium.Sdk.Types;

/// <summary>
/// Epoch number
/// </summary>
public record struct Epoch(ulong Count)
{
    internal static Epoch From(Grpc.V2.Epoch epoch) => new(epoch.Value);
}
