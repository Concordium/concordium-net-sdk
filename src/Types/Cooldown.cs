using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// The stake on the account that is in cooldown.
/// </summary>
/// <param name="EndTime">The time when the cooldown period ends.</param>
/// <param name="Amount">The amount that is in cooldown and set to be released at the end of the cooldown period.</param>
/// <param name="Status">The status of the cooldown.</param>
public sealed record Cooldown(
    DateTimeOffset EndTime,
    CcdAmount Amount,
    CooldownStatus Status
)
{
    internal static Cooldown From(Grpc.V2.Cooldown cooldown) => new(
        cooldown.EndTime.ToDateTimeOffset(),
        CcdAmount.From(cooldown.Amount),
        cooldown.Status.Into()
    );
}
