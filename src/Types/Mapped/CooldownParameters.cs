namespace Concordium.Sdk.Types.Mapped;

/// <param name="PoolOwnerCooldown">
/// Duration that pool owners must cooldown
/// when reducing their equity capital or closing the pool.
/// </param>
/// <param name="DelegatorCooldown">
/// Duration that a delegator must cooldown
/// when reducing their delegated stake.
/// </param>
public record CooldownParameters(TimeSpan PoolOwnerCooldown, TimeSpan DelegatorCooldown)
{
    internal static CooldownParameters From(Grpc.V2.CooldownParametersCpv1 coolDown) =>
        new(
            TimeSpan.FromSeconds((double)coolDown.PoolOwnerCooldown.Value),
            TimeSpan.FromSeconds((double)coolDown.DelegatorCooldown.Value));
}
