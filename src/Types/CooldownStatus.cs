using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// The status of a cooldown.
/// </summary>
/// <remarks>
/// When stake is removed from a baker or delegator
/// (from protocol version 7) it first enters the pre-pre-cooldown state.
/// The next time the stake snaphot is taken (at the epoch transition before
/// a payday) it enters the pre-cooldown state. At the subsequent payday, it
/// enters the cooldown state. At the payday after the end of the cooldown
/// period, the stake is finally released.
/// </remarks>
public enum CooldownStatus
{
    /// <summary>
    /// The amount is in cooldown and will expire at the specified time, becoming available
    /// at the subsequent pay day.
    /// </summary>
    Cooldown,
    /// <summary>
    /// The amount will enter cooldown at the next pay day. The specified end time is
    /// projected to be the end of the cooldown period, but the actual end time will be
    /// determined at the payday, and may be different if the global cooldown period
    /// changes.
    /// </summary>
    PreCooldown,
    /// <summary>
    /// The amount will enter pre-cooldown at the next snapshot epoch (i.e. the epoch
    /// transition before a pay day transition). As with pre-cooldown, the specified
    /// end time is projected, but the actual end time will be determined later.
    /// </summary>
    PrePreCooldown
}

internal static class CooldownStatusFactory
{
    internal static CooldownStatus Into(this Grpc.V2.Cooldown.Types.CooldownStatus status) =>
        status switch
        {
            Grpc.V2.Cooldown.Types.CooldownStatus.Cooldown => CooldownStatus.Cooldown,
            Grpc.V2.Cooldown.Types.CooldownStatus.PreCooldown => CooldownStatus.PreCooldown,
            Grpc.V2.Cooldown.Types.CooldownStatus.PrePreCooldown => CooldownStatus.PrePreCooldown,
            _ => throw new MissingEnumException<Grpc.V2.Cooldown.Types.CooldownStatus>(status)
        };
}
