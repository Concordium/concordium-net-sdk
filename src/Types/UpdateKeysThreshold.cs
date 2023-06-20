namespace Concordium.Sdk.Types;

/// <summary>
/// A lower bound on the number of signatures needed to sign a valid update
/// message of a particular type. This is never 0.
/// </summary>
/// <param name="Threshold"></param>
public readonly record struct UpdateKeysThreshold(ushort Threshold)
{
    internal static UpdateKeysThreshold From(Grpc.V2.UpdateKeysThreshold threshold) => new((ushort)threshold.Value);
}
