namespace Concordium.Sdk.Types;

/// <summary>
/// Length of a reward period in epochs.
/// Must always be a strictly positive integer.
/// </summary>
public readonly record struct RewardPeriodLength(Epoch RewardPeriodEpochs)
{
    internal static RewardPeriodLength From(Grpc.V2.RewardPeriodLength length) => new(Epoch.From(length.Value));
}
