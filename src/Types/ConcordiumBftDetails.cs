using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Parameters pertaining to the Concordium BFT consensus.
/// </summary>
/// <param name="CurrentTimeoutDuration">The current duration to wait before a round times out.</param>
/// <param name="Round">The current round.</param>
/// <param name="Epoch">The current epoch.</param>
/// <param name="TriggerBlockTime">
/// The first block in the epoch with timestamp at least this is considered
/// to be the trigger block for the epoch transition.
/// </param>
public sealed record ConcordiumBftDetails(TimeSpan CurrentTimeoutDuration, Round CurrentRound, Epoch Epoch, DateTimeOffset TriggerBlockTime)
{
    internal static bool TryFrom(Grpc.V2.ConsensusInfo info, out ConcordiumBftDetails? details)
    {
        if (info.CurrentTimeoutDuration == null || info.CurrentRound == null ||
            info.CurrentEpoch == null || info.TriggerBlockTime == null)
        {
            details = null;
            return false;
        }

        details = new ConcordiumBftDetails(
            info.CurrentTimeoutDuration.ToTimeSpan(),
            Round.From(info.CurrentRound),
            Epoch.From(info.CurrentEpoch),
            info.TriggerBlockTime.ToDateTimeOffset());
        return true;
    }
}
