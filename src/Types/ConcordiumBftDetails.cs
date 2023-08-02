using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Parameters pertaining to the Concordium BFT consensus.
/// </summary>
/// <param name="CurrentTimeoutDuration">The current duration to wait before a round times out.</param>
/// <param name="CurrentRound">The current round.</param>
/// <param name="CurrentEpoch">The current epoch.</param>
/// <param name="TriggerBlockTime">
/// The first block in the epoc with a timestamp equal to or later than this timestamp, is considered
/// to be the trigger block for the epoch transition.
/// </param>
public sealed record ConcordiumBftDetails(TimeSpan CurrentTimeoutDuration, Round CurrentRound, Epoch CurrentEpoch, DateTimeOffset TriggerBlockTime)
{
    internal static ConcordiumBftDetails? From(Grpc.V2.ConsensusInfo info)
    {
        if (info.CurrentTimeoutDuration == null || info.CurrentRound == null ||
            info.CurrentEpoch == null || info.TriggerBlockTime == null)
        {
            return null;
        }

        return new ConcordiumBftDetails(
            info.CurrentTimeoutDuration.ToTimeSpan(),
            Round.From(info.CurrentRound),
            Epoch.From(info.CurrentEpoch),
            info.TriggerBlockTime.ToDateTimeOffset());
    }
}
