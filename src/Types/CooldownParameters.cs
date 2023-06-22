namespace Concordium.Sdk.Types;

/// <param name="PoolOwnerCooldown">
/// Duration that pool owners must cooldown
/// when reducing their equity capital or closing the pool.
/// </param>
/// <param name="DelegatorCooldown">
/// Duration that a delegator must cooldown
/// when reducing their delegated stake.
/// </param>
public sealed record CooldownParameters(TimeSpan PoolOwnerCooldown, TimeSpan DelegatorCooldown)
{
    internal static CooldownParameters From(Grpc.V2.CooldownParametersCpv1 coolDown) =>
        new(
            TimeSpan.FromSeconds(coolDown.PoolOwnerCooldown.Value),
            TimeSpan.FromSeconds(coolDown.DelegatorCooldown.Value));
}
