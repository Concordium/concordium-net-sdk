namespace Concordium.Sdk.Types;

/// <summary>
/// Parameters that determine timeouts in the consensus protocol used from protocol version 6.
/// </summary>
/// <param name="Duration">The base value for triggering a timeout.</param>
/// <param name="Increase">Factor for increasing the timeout. Must be greater than 1.</param>
/// <param name="Decrease">Factor for decreasing the timeout. Must be between 0 and 1.</param>
public sealed record TimeoutParameters(TimeSpan Duration, Ratio Increase, Ratio Decrease)
{
    internal static TimeoutParameters From(Grpc.V2.TimeoutParameters parameters) =>
        new(
            TimeSpan.FromMilliseconds((long)parameters.TimeoutBase.Value),
            Ratio.From(parameters.TimeoutIncrease),
            Ratio.From(parameters.TimeoutDecrease));
}
